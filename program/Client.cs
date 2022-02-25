using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Program
{
    struct HttpHeaders
    {
        public string Method;
        public string RealPath;
        public string File;
        public string Redirect;
        public string ContentType;
        public string ContentLength;
        public string QueryString;
        public string Coockie;
        public string SetCoockie;
        public string Status;
        public string Location;
        public string Cgi;
        public bool UseCGI;
        public string Domain;
        public static string WWW = "www";
        public string InterpreterPath;

        private static Global _global;
        private static string _redirectStatus = false.ToString();
        private static string _CGI = "CGI";

        public static HttpHeaders Parse(Global Global, string headers)
        {
            _global = Global;
            return Parse(headers);
        }
        public static HttpHeaders Parse(string headers)
        {
            HttpHeaders result = new HttpHeaders();
            result.Method = Regex.Match(headers, @"\A\w[a-zA-Z]+", RegexOptions.Multiline).Value;
            result.Domain = Regex.Match(headers, @"(?<=Host:\s)[\w\S]+", RegexOptions.Multiline).Value;
            if (!_global.MultipleSite || result.Domain == _global.Ipv4 || result.Domain == _global.Server.Ip.ToString())
                result.Domain = "";
            result.File = Regex.Match(headers, @"(?<=\w\s)([\W\w]+)(?=\sHTTP)", RegexOptions.Multiline).Value;
            result.Redirect = _redirectStatus;
            result.QueryString = Regex.Match(headers, @"(?<=[\?\n])([^\:]+?[&%\=])+[\W\w]\S", RegexOptions.Multiline).Value;
            result.QueryString = Regex.Match(result.QueryString, @"(?<=[\?\n])([^\:]+?[&%\=])+[\W\w]", RegexOptions.Multiline).Value;
            result.ContentType = Regex.Match(headers, @"(?<=^Content-Type:\s)[\S\s]+?(?=[\s]{0,}$)", RegexOptions.Multiline).Value;
            result.ContentLength = Regex.Match(headers, @"(?<=^Content-Length:\s)[\S\s]+?(?=[\s]{0,}$)", RegexOptions.Multiline).Value;
            result.Cgi = _CGI;
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
            foreach (var i in _global.Alias)
                if (i.Value == result.Domain)
                    result.Domain = i.Key;
            result.RealPath = $"{AppDomain.CurrentDomain.BaseDirectory}{WWW}/{result.Domain}{result.File}";
            if (result.File == "/" || result.File == "\\" || result.File == " " || result.File == "" || result.File.Substring(result.File.Length-1) == "/" || result.File.Substring(result.File.Length - 1) == @"\")
                foreach (var ext in _global.Server.Extensions)
                    if (System.IO.File.Exists($"{result.RealPath}index{ext}"))
                    {
                        result.RealPath += $"index{ext}";
                        result.File = $"index{ext}";
                        break;
                    }
            return result;
        }
        public static HttpHeaders ParseCGI(HttpHeaders head, string headers)
        {
            HttpHeaders result = new HttpHeaders();
            List<string> list = new List<string>();
            result.Status = Regex.Match(headers, @"(?<=Status:\s)\d+").Value;
            result.Domain = Regex.Match(headers, @"(?<=Host:\s)[\w\S]+", RegexOptions.Multiline).Value;
            if (!_global.MultipleSite || result.Domain == _global.Ipv4 || result.Domain == _global.Server.Ip.ToString())
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
            foreach (var i in _global.Alias)
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
        private Socket _client;
        private Server _server;
        private Interpreter _interpreter = new Interpreter();
        private HttpHeaders _headers;
        public Client(Socket client, Server server)
        {
            _client = client;
            _server = server;
            _client.Ttl = 10;
            string request = "";
            byte[] data = new byte[_client.ReceiveBufferSize];
            _client.Receive(data);
            request += Encoding.UTF8.GetString(data);

            // Проверка на пустой запрос
            if (request == "")
            {
                _client.Close();
                return;
            }

            // Парсим заголовки
            _headers = HttpHeaders.Parse(_server.Global, request);
            if (_headers.Method == "" || _headers.RealPath == "")
            {
                _client.Close();
                return;
            }

            // Вывод информацию о подключении
            Console.WriteLine($@"[{_client.RemoteEndPoint}]
Path: {_headers.RealPath}
Date: {DateTime.Now}");

            if (_headers.RealPath.IndexOf("..") != -1)
            {
                SendError(ServerError.NotFound);
                _client.Close();
                return;
            }


            if (File.Exists(_headers.RealPath)) GetSheet(_headers);
            else SendError(ServerError.NotFound);
        }
        public void GetSheet(HttpHeaders head)
        {
            try
            {
                string extention = HttpHeaders.FileExtention(head.RealPath);
                if (extention == "py" || extention == "php")
                {
                    string html = AnyFile(head);
                    int length = html.Length;
                    HttpHeaders file = HttpHeaders.ParseCGI(head, html);
                    int status = 200;
                    string contentType = GetContentType(head);
                    if (int.TryParse(file.Status, out var st))
                        status = st;
                    string headers = $"HTTP/1.1 {status} OK\nContent-type: {contentType}\nContent-Length: {length}";
                    if (file.SetCoockie != null && file.SetCoockie != "")
                        headers += $"\n{file.SetCoockie}";
                    //Console.WriteLine(file.SetCoockie);
                    if (file.Location != null && file.Location != "")
                        headers += $"\n{file.Location}";
                    headers += $"\n\n{html}";
                    // OUTPUT HEADERS
                    byte[] dataHeaders = Encoding.UTF8.GetBytes(headers);
                    //client.GetStream().Write(data_headers, 0, data_headers.Length);
                    _client.Send(dataHeaders, dataHeaders.Length, SocketFlags.None);
                }
                else
                {
                    string contentType = GetContentType(head);
                    FileStream fs = new FileStream(head.RealPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    string headers = $"HTTP/1.1 200 OK\nContent-type: {contentType}\nContent-Length: {fs.Length}\n\n";
                    // OUTPUT HEADERS
                    byte[] dataHeaders = Encoding.UTF8.GetBytes(headers);
                    _client.Send(dataHeaders, dataHeaders.Length, SocketFlags.None);
                    // OUTPUT CONTENT
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                    fs.Close();
                    _client.Send(data, data.Length, SocketFlags.None);
                }
                _client.Shutdown(SocketShutdown.Receive);
                
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Func: GetSheet()    link: {head.RealPath}\nException: {ex}\nMessage: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            finally
            {
                SendError(ServerError.InternalServerError);
                _client.Close();
            }
        }
        //TODO CGI
        string AnyFile(HttpHeaders head)
        {
            string result = "";
            string ext = HttpHeaders.FileExtention(head.RealPath);
            string interpretator = "";
            string type = "";
            foreach (var i in _server.Global.Interpreters)
            {
                if (i.Value.Name == ext)
                {
                    if (head.UseCGI || head.Coockie != "")
                    {
                        if (i.Value.Type == "cgi")
                        {
                            head.InterpreterPath = $"{AppDomain.CurrentDomain.BaseDirectory}{i.Value.Path}";
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
            if (head.UseCGI) result = Interpreter.UseCGI(head);
            else result = _interpreter.UseInterpreter(interpretator, head.RealPath);

            return result;
        }
        string GetContentType(HttpHeaders head)
        {
            string result = "";
            string format = head.RealPath.Substring(head.RealPath.LastIndexOf("."));
            format = format.Replace(".", "").ToLower();
            switch (format)
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
                case "atom":
                    result = $"application/atom+xml";
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

            return result;
        }
        public void SendError(ServerError error)
        {
            string html = $"<html><head><title></title></head><body><h1>Error {(int)error}</h1><div>{error}</div></body></html>";
            string headers = $"HTTP/1.1 {(int)error} OK\nContent-type: text/html\nContent-Length: {html.Length}\n\n{html}";
            byte[] data = Encoding.UTF8.GetBytes(headers);
            _client.Send(data, data.Length, SocketFlags.None);
            _client.Close();
        }
        public void SendError(ServerError error, string message)
        {
            string html = $"<html><head><title></title></head><body><h1>Error {(int)error}</h1><div>{message}</div></body></html>";
            string headers = $"HTTP/1.1 {(int)error} OK\nContent-type: text/html\nContent-Length: {html.Length}\n\n{html}";
            byte[] data = Encoding.UTF8.GetBytes(headers);
            _client.Send(data, data.Length, SocketFlags.None);
            _client.Close();
        }
    }
    public enum ServerError
    {
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500
    }
}
