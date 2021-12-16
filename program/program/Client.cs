using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace program
{
    struct HTTPHeaders
    {
        public string Method;
        public string RealPath;
        public string File;
        private static string RedirectStatus = false.ToString();
        public string Redirect;
        public string ContentType;
        public string ContentLength;
        public string QueryString;
        public string Coockie;
        public string SetCoockie;
        public string Status;
        public string Location;
        private static string CGI = "CGI";
        public string Cgi;
        public bool UseCGI;

        public string Domain;
        public static string WWW = "www";
        public string Interpreter_path;

        private static Global global;
        
        public static HTTPHeaders Parse(Global Global, string headers)
        {
            global = Global;
            return Parse(headers);
        }
        public static HTTPHeaders Parse(string headers)
        {
            HTTPHeaders result = new HTTPHeaders();
            result.Method = Regex.Match(headers, @"\A\w[a-zA-Z]+", RegexOptions.Multiline).Value;
            result.Domain = Regex.Match(headers, @"(?<=Host:\s)[\w\S]+", RegexOptions.Multiline).Value;
            if (result.Domain == global.IPv4)
                result.Domain = "";
            result.File = Regex.Match(headers, @"(?<=\w\s)([\W\w]+)(?=\sHTTP)", RegexOptions.Multiline).Value;
            result.Redirect = RedirectStatus;
            result.QueryString = Regex.Match(headers, @"(?<=[\?\n])([^\:]+?[&%\=])+[\W\w]\S", RegexOptions.Multiline).Value;
            result.QueryString = Regex.Match(result.QueryString, @"(?<=[\?\n])([^\:]+?[&%\=])+[\W\w]", RegexOptions.Multiline).Value;
            result.ContentType = Regex.Match(headers, @"(?<=^Content-Type:\s)[\S\s]+?(?=[\s]{0,}$)", RegexOptions.Multiline).Value;
            result.ContentLength = Regex.Match(headers, @"(?<=^Content-Length:\s)[\S\s]+?(?=[\s]{0,}$)", RegexOptions.Multiline).Value;
            result.Cgi = CGI;
            //result.Coockie = Regex.Match(headers, @"(?<=Cookie:\s)([\W\w]+?\n)").Value;
            result.Coockie = Regex.Match(headers, @"(Cookie:\s)([\W\w]+?\n)").Value;
            result.QueryString += result.Coockie;
            if (result.QueryString.Length > 0)
            {
                result.UseCGI = true;
                result.QueryString = result.QueryString.Replace(" ", "");
            }     
            if (result.QueryString != "" && result.File.IndexOf(result.QueryString) != -1)
                result.File = result.File.Replace(result.QueryString, "");
            if (result.File.IndexOf("?") != -1)
                result.File = result.File.Replace("?", "");
            foreach (var i in global.Alias)
                if (i.Value == result.Domain)
                    result.Domain = i.Key;
            result.RealPath = $"{AppDomain.CurrentDomain.BaseDirectory}{WWW}/{result.Domain}{result.File}";
            if (result.File == "/" || result.File == "\\" || result.File == " " || result.File == "" || result.File.Substring(result.File.Length-1) == "/" || result.File.Substring(result.File.Length - 1) == @"\")
                foreach (var ext in global.Server.Extensions)
                    if (System.IO.File.Exists($"{result.RealPath}index{ext}"))
                    {
                        result.RealPath += $"index{ext}";
                        result.File = $"index{ext}";
                        break;
                    }
            return result;
        }
        public static HTTPHeaders ParseCGI(HTTPHeaders head, string headers)
        {
            HTTPHeaders result = new HTTPHeaders();
            List<string> list = new List<string>();
            result.Status = Regex.Match(headers, @"(?<=Status:\s)\d+").Value;
            result.Domain = Regex.Match(headers, @"(?<=Host:\s)[\w\S]+", RegexOptions.Multiline).Value;
            if (result.Domain == global.IPv4)
                result.Domain = "";
            result.Location = Regex.Match(headers, @"(Location:\s[\W\w]+?)$").Value;
            result.File = Regex.Match(headers, @"(?<=\w\s)([\W\w]+)(?=\sHTTP)", RegexOptions.Multiline).Value;
            if(result.File == "")
                result.File = Regex.Match(headers, @"(?<=Location:\s)([\W\w]+?)$", RegexOptions.Multiline).Value;
            result.Redirect = true.ToString();
            result.QueryString = Regex.Match(headers, @"(?<=[\?\n])([^\:]+?[&%\=])+[\W\w]", RegexOptions.Multiline).Value;
            result.QueryString = Regex.Match(result.QueryString, @"(?<=[\?\n])([^\:]+?[&%\=])+[\W\w]", RegexOptions.Multiline).Value;
            result.ContentType = Regex.Match(headers, @"(?<=^Content-Type:\s)[\S\s]+?(?=[\s]{0,}$)", RegexOptions.Multiline).Value;
            result.ContentLength = Regex.Match(headers, @"(?<=^Content-Length:\s)[\S\s]+?(?=[\s]{0,}$)", RegexOptions.Multiline).Value;
            if (headers.IndexOf("Set-Cookie") != -1)
            {
                MatchCollection collect = Regex.Matches(headers, @"(Set-Cookie:\s[\w\W]+?$)", RegexOptions.Multiline);
                foreach (Match i in collect)
                    list.Add(i.Value);
                foreach(var i in list)
                {
                    result.SetCoockie += i+"\n";
                }
            }
            if (result.QueryString != "" && result.File.IndexOf(result.QueryString) != -1)
                result.File = result.File.Replace(result.QueryString, "");
            if (result.File.IndexOf("?") != -1)
                result.File = result.File.Replace("?", "");
            foreach (var i in global.Alias)
                if (i.Value == result.Domain)
                    result.Domain = i.Key;
            result.RealPath = $"{AppDomain.CurrentDomain.BaseDirectory}{WWW}/{result.Domain}{result.File}";
            return result;
        }
        public static string FileExtention(string file)
        {
            return Regex.Match(file, @"(?<=[\W])\w+(?=[\W]{0,}$)").Value;
        }
    }
    class Client
    {
        Socket client;
        Server server;
        Interpreter interpreter = new Interpreter();
        HTTPHeaders Headers;
        public Client(Socket c, Server s)
        {
            client = c;
            server = s;
            client.Ttl = 8;
            string request = "";
            byte[] data = new byte[1024];
            client.Receive(data);
            request += Encoding.UTF8.GetString(data);
            // Проверка на пустой запрос
            if (request == "")
            {
                client.Close();
                return;
            }
            // Парсим заголовки
            Headers = HTTPHeaders.Parse(server.global, request);
            if (Headers.Method == "" || Headers.RealPath == "")
            {
                client.Close();
                return;
            }
            //Console.WriteLine(request);
            // Вывод информацию о подключении
            Console.WriteLine($@"[{client.RemoteEndPoint}]
Path: {Headers.RealPath}
Date: {DateTime.Now}");
            if (Headers.RealPath.IndexOf("..") != -1)
            {
                SendError(404);
                client.Close();
                return;
            }
            if (File.Exists(Headers.RealPath))
                GetSheet(Headers);
            else
                SendError("Not Found", 404);
        }
        ~Client()
        {
            client.Close();
            GC.Collect(2, GCCollectionMode.Forced);
        }
        // TODO - REWRITE
        public void GetSheet(HTTPHeaders head)
        {
            try
            {
                string extention = HTTPHeaders.FileExtention(head.RealPath);
                lock (new object())
                {
                    if (extention == "py" || extention == "php")
                    {
                        string html = AnyFile(head);
                        int length = html.Length;
                        HTTPHeaders file = HTTPHeaders.ParseCGI(head, html);
                        int status = 200;
                        string content_type = GetContentType(head);
                        if (int.TryParse(file.Status, out var st))
                            status = st;
                        string headers = $"HTTP/1.1 {status} OK\nContent-type: {content_type}\nContent-Length: {length}";
                        if (file.SetCoockie != null && file.SetCoockie != "")
                            headers += $"\n{file.SetCoockie}";
                        //Console.WriteLine(file.SetCoockie);
                        if (file.Location != null && file.Location != "")
                            headers += $"\n{file.Location}";
                        headers += $"\n\n{html}";
                        // OUTPUT HEADERS
                        byte[] data_headers = Encoding.UTF8.GetBytes(headers);
                        //client.GetStream().Write(data_headers, 0, data_headers.Length);
                        client.Send(data_headers, data_headers.Length, SocketFlags.None);
                    }
                    else
                    {
                        
                        string content_type = GetContentType(head);
                        FileStream fs = new FileStream(head.RealPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        string headers = $"HTTP/1.1 200 OK\nContent-type: {content_type}\nContent-Length: {fs.Length}\n\n";
                        // OUTPUT HEADERS
                        byte[] data_headers = Encoding.UTF8.GetBytes(headers);
                        client.Send(data_headers, data_headers.Length, SocketFlags.None);
                        // OUTPUT CONTENT
                        while (fs.Position < fs.Length)
                        {
                            byte[] data = new byte[1024];
                            int length = fs.Read(data, 0, data.Length);
                            client.Send(data, data.Length, SocketFlags.None);
                        }
                    }
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                client.Close();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Func: GetSheet()    link: {head.RealPath}\nException: {ex}\nMessage: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        //TODO CGI
        string AnyFile(HTTPHeaders head)
        {
            string result = "";
            lock (new object())
            {
                string ext = HTTPHeaders.FileExtention(head.RealPath);
                string interpretator = "";
                string type = "";
                foreach (var i in server.global.Interpreters)
                {
                    if (i.Value.Name == ext)
                    {
                        if(head.UseCGI || head.Coockie != "")
                        {
                            if (i.Value.Type == "cgi")
                            {
                                head.Interpreter_path = $"{AppDomain.CurrentDomain.BaseDirectory}{i.Value.Path}";
                                type = i.Value.Type;
                            }
                        }
                        else
                        {
                            if (i.Value.Type == "int")
                            {
                                interpretator = $"{AppDomain.CurrentDomain.BaseDirectory}{i.Value.Path}";
                                type = i.Value.Type;
                            }
                        }
                    }
                }
                if(head.UseCGI)
                    result = Interpreter.UseCGI(head);
                else
                    result = interpreter.UseInterpreter(interpretator, head.RealPath);
            }
            //Console.WriteLine($"res =>\n{result}");
            return result;
        }
        string GetContentType(HTTPHeaders head)
        {
            string result = "";
            if(File.Exists(head.RealPath))
            {
                string format = head.RealPath.Substring(head.RealPath.LastIndexOf("."));
                format = format.Replace(".","").ToLower();
                switch(format)
                {
                    //application
                    case "xml":
                    case "json":
                    case "ogg":
                    case "pdf":
                    case "postscript":
                    case "zip":
                    case "gzip":
                    case "doc":
                        result = $"application/{format}";
                        break;
                    case "EDI":
                    case "EDI-X12":
                    case "EDIFACT":
                        result = $"application/{format}";
                        break;
                    case "atom": result = $"application/atom+xml";
                        break;
                    case "soap":
                        result = $"application/soap+xml";
                        break;
                    case "woff":
                        result = $"application/font-woff";
                        break;
                    case "xhtml":
                        result = $"application/xhtml+xml";
                        break;
                    case "torrent":
                    case "bittorrent":
                        result = $"application/x-bittorrent";
                        break;
                    case "tex":
                        result = $"application/x-tex";
                        break;
                    //audio
                    case "basic":
                    case "l24":
                    case "mp4":
                    case "acc":
                    case "mpeg":
                    case "vorbis":
                    case "webm":
                        result = $"audio/{format}";
                        break;
                    case "wma":
                        result = $"audio/x-ms-wma";
                        break;
                    case "rm":
                        result = $"audio/vnd.rn-realaudio";
                        break;
                    case "wav":
                    case "wave":
                        result = $"audio/vnd.wave";
                        break;
                    //image
                    case "gif":
                    case "jpeg":
                    case "pjpeg":
                    case "png":
                    case "tiff":
                    case "webp":
                        result = $"image/{format}";
                        break;
                    case "svg":
                        result = $"image/svg+xml";
                        break;
                    case "ico":
                        result = $"image/vnd.microsoft.icon";
                        break;
                    case "wbmp":
                        result = $"image/vnd.map.wbmp";
                        break;
                    case "jpg":
                        result = $"image/jpeg";
                        break;
                    //message
                    case "eml":
                    case "mime":
                    case "mth":
                    case "mhtml":
                        result = $"message/ftc822";
                        break;
                    //model
                    case "iges":
                    case "mesh":
                    case "vrml":
                    case "wrl":
                    case "silo":
                    case "igs":
                    case "msh":
                    case "example":
                        result = $"model/{format}";
                        break;
                    case "x3db":
                        result = $"model/x3d+binary";
                        break;
                    case "x3dv":
                        result = $"model/x3d+vrml";
                        break;
                    case "x3d":
                        result = $"model/x3d+xml";
                        break;
                    //multipart
                    //text
                    case "cmd":
                    case "css":
                    case "cvs":
                    case "html":
                    case "plain":
                    case "markdown":
                    case "md":
                        result = $"text/{format}";
                        break;
                    case "javascript":
                    case "js":
                        result = $"text/javascript";
                        break;
                    case "php":
                        result = $"text/html";
                        break;
                    case "htm":
                        result = $"text/html";
                        break;
                    case "py":
                        result = $"text/html";
                        break;
                    //video
                    //vnd
                    //x
                    //x-pkcs
                    default:
                        result = "application/unknown";
                        break;
                }
                //Console.WriteLine($"link: {link} format: {format} content-type: {result}");
            }
            else
            {
                result = "application/unknown";
            }
            return result;
        }
        public void SendError(int code)
        {
            string html = $"<html><head><title></title></head><body><h1>Error {code}</h1></body></html>";
            string headers = $"HTTP/1.1 {code} OK\nContent-type: text/html\nContent-Length: {html.Length}\n\n{html}";
            byte[] data = Encoding.UTF8.GetBytes(headers);
            client.Send(data, data.Length, SocketFlags.None);
            client.Close();
        }
        public void SendError(string message, int code)
        {
            string html = $"<html><head><title></title></head><body><h1>Error {code}</h1><div>{message}</div></body></html>";
            string headers = $"HTTP/1.1 {code} OK\nContent-type: text/html\nContent-Length: {html.Length}\n\n{html}";
            byte[] data = Encoding.UTF8.GetBytes(headers);
            client.Send(data, data.Length, SocketFlags.None);
            client.Close();
        }
    }
}
