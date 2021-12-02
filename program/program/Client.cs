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
        private static string CGI = "CGI";
        public string Cgi;
        public bool UseCGI;

        public string Domain;
        public static string WWW = "www";
        public Match FirstHead;
        public string Interpreter_path;

        public bool Exist;
        private static Global global;
        
        public static HTTPHeaders Parse(Global Global, string headers)
        {
            global = Global;
            return Parse(headers);
        }
        public static HTTPHeaders Parse(string headers)
        {
            HTTPHeaders result = new HTTPHeaders();
            result.FirstHead = Regex.Match(headers, @"^\w+\s+([^\s\?]+)[^\s]*\s+HTTP/.*|", RegexOptions.Multiline);
            result.Method = Regex.Match(headers, @"\A\w[a-zA-Z]+", RegexOptions.Multiline).Value;
            result.Domain = Regex.Match(headers, @"(?<=Host:\s)[\w]+", RegexOptions.Multiline).Value;
            result.File = Regex.Match(headers, @"(?<=\w\s)([\Wa-zA-Z0-9]+)(?=\sHTTP)", RegexOptions.Multiline).Value;
            result.Redirect = RedirectStatus;
            result.QueryString = Regex.Match(headers, @"(?<=[\?\n])([^\:]+?[&%\=])+[\W\w]", RegexOptions.Multiline).Value;
            result.ContentType = Regex.Match(headers, @"(?<=^Content-Type:\s)[\S\s]+?(?=[\s]{0,}$)", RegexOptions.Multiline).Value;
            result.ContentLength = Regex.Match(headers, @"(?<=^Content-Length:\s)[\S\s]+?(?=[\s]{0,}$)", RegexOptions.Multiline).Value;
            result.Cgi = CGI;
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
            if (result.File == "/" || result.File == "\\" || result.File == " " || result.File == "" || result.File[result.File.Length - 1] == '/' || result.File[result.File.Length - 1] == '\\')
                foreach (var ext in global.Server.Extensions)
                    if (System.IO.File.Exists($"{result.RealPath}index{ext}"))
                    {
                        result.RealPath += $"index{ext}";
                        result.File = $"index{ext}";
                        break;
                    }
            
            result.Exist = System.IO.File.Exists(result.RealPath);
            return result;
        }
        public static string FileExtention(string file)
        {
            return Regex.Match(file, @"(?<=[\W])\w+(?=[\W]{0,}$)").Value;
        }
    }
    class Client
    {
        TcpClient client;
        Server server;
        Interpreter interpreter = new Interpreter();
        HTTPHeaders Headers;
        public Client(TcpClient c, Server s)
        {
            client = c;
            server = s;
            client.SendTimeout = 20000;
            client.ReceiveTimeout = 20000;
            string FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}{HTTPHeaders.WWW}";
            NetworkStream stream = client.GetStream();
            string request = "";
            byte[] data = new byte[1024];
            while (stream.DataAvailable)
            {
                stream.Read(data, 0, data.Length);
                request += Encoding.UTF8.GetString(data);
            }
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
            // Вывод информацию о подключении
            Console.WriteLine($@"[{client.Client.RemoteEndPoint}]
Path: {Headers.RealPath}
Date: {DateTime.Now}");
            if (Headers.RealPath.IndexOf("..") != -1)
            {
                SendError(404);
                client.Close();
                return;
            }
            if (Headers.Exist)
                GetSheet(Headers);
            else
                SendError(404);
            client.Close();
        }
        ~Client()
        {
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
                        //Console.WriteLine(html);
                        string content_type = GetContentType(head);
                        int length = html.Length;
                        string headers = $"HTTP/1.1 200 OK\nContent-type: {content_type}\nContent-Length: {length}\n\n{html}";
                        // OUTPUT HEADERS
                        byte[] data_headers = Encoding.UTF8.GetBytes(headers);
                        client.GetStream().Write(data_headers, 0, data_headers.Length);
                    }
                    else
                    {
                        
                        string content_type = GetContentType(head);
                        FileStream fs = new FileStream(head.RealPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        string headers = $"HTTP/1.1 200 OK\nContent-type: {content_type}\nContent-Length: {fs.Length}\n\n";
                        // OUTPUT HEADERS
                        byte[] data_headers = Encoding.UTF8.GetBytes(headers);
                        client.GetStream().Write(data_headers, 0, data_headers.Length);
                        // OUTPUT CONTENT
                        while (fs.Position < fs.Length)
                        {
                            byte[] data = new byte[1024];
                            int length = fs.Read(data, 0, data.Length);
                            client.GetStream().Write(data, 0, length);
                        }
                    }
                }
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Func: GetSheet()    link: {head.RealPath}\nException: {ex}/nMessage: {ex.Message}");
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
                        if(head.UseCGI)
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
            string headers = $"HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length: {html.Length}\n\n{html}";
            byte[] data = Encoding.UTF8.GetBytes(headers);
            client.GetStream().Write(data, 0, data.Length);
            client.Close();
        }
        public void SendError(string message, int code)
        {
            string html = $"<html><head><title></title></head><body><h1>Error {code}</h1><div>{message}</div></body></html>";
            string headers = $"HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length: {html.Length}\n\n{html}";
            byte[] data = Encoding.UTF8.GetBytes(headers);
            client.GetStream().Write(data, 0, data.Length);
            client.Close();
        }
    }
}
