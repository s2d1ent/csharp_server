﻿using System;
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
        public string ReadlPath;
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

        public static HTTPHeaders Parse(string headers)
        {
            HTTPHeaders result = new HTTPHeaders();
            result.FirstHead = Regex.Match(headers, @"^\w+\s+([^\s\?]+)[^\s]*\s+HTTP/.*|", RegexOptions.Multiline);
            result.Method = Regex.Match(headers, @"\A\w[a-zA-Z]+", RegexOptions.Multiline).Value;
            result.Domain = Regex.Match(headers, @"Host:\s[a-z|A-Z|0-9]+", RegexOptions.Multiline).Value.Replace("Host:", "").Replace(" ", "");
            result.File = Regex.Match(headers, @"(?<=\w\s)([\Wa-zA-Z0-9]+)(?=\sHTTP)", RegexOptions.Multiline).Value;
            result.Redirect = RedirectStatus;
            result.QueryString = Regex.Match(headers, @"(?<=[\?\n])([^\:]+?[&%\=])+\w+", RegexOptions.Multiline).Value;
            result.ContentType = Regex.Match(headers, @"(?<=^Content-Type:\s)[\S\s]+?(?=[\s]{0,}$)", RegexOptions.Multiline).Value;
            result.ContentLength = Regex.Match(headers, @"(?<=^Content-Length:\s)[\S\s]+?(?=[\s]{0,}$)", RegexOptions.Multiline).Value;
            if (result.QueryString != "" && result.QueryString != "" || result.QueryString != null)
                result.QueryString = result.QueryString.Replace(" ", "");
            if (result.File.IndexOf("?") != -1)
                result.File = result.File.Replace("?", "");
            result.ReadlPath = $"{AppDomain.CurrentDomain.BaseDirectory}{WWW}/{result.Domain}{result.File}";
            result.Cgi = CGI;
            return result;
        }
    }
    class Client
    {
        TcpClient client;
        Server server;
        Interpreter interpreter = new Interpreter();
        HTTPHeaders Headers;
        //string site = @"C:\Users\Admin\Desktop\csharp_server\www";
        public Client(TcpClient c, Server s)
        {
            client = c;
            server = s;
            string FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}{HTTPHeaders.WWW}";
            NetworkStream stream = client.GetStream();
            string request = "";
            byte[] data = new byte[1024];
            while (stream.DataAvailable)
            {
                stream.Read(data, 0, data.Length);
                request += Encoding.UTF8.GetString(data);
            }
            // Парсим заголовки
            Headers = HTTPHeaders.Parse(request);
            if (Headers.FirstHead == Match.Empty)
            {
                Console.WriteLine($"ReqMatch = Empty");
                SendError(400);
                return;
            }
            string content_type = Regex.Match(request, @"Content-Type:\s[a-z]+\/[a-z]-[a-z]+-[a-z]+-[a-z]+").Value;
            //Console.WriteLine(ReqMatch);
            //Console.WriteLine(request);
            foreach (var i in server.global.Alias)
                if(i.Value == Headers.Domain)
                    Headers.Domain = i.Key;
            if (Headers.File == "/" || Headers.File == "\\" || Headers.File == " " || Headers.File == "" || Headers.File[Headers.File.Length - 1] == '/' || Headers.File[Headers.File.Length - 1] == '\\')
            {
                foreach(var ext in server.Extensions)
                    if (File.Exists($"{HTTPHeaders.WWW}/{Headers.Domain}/index{ext}"))
                    {
                        Headers.File = $"/index{ext}";
                        break;
                    }
            }
            if (Headers.File.IndexOf("..") != -1)
            {
                SendError(404);
                client.Close();
                return;
            }
            GetSheet(Headers);
            //Console.WriteLine($"Path: {FilePath} - Exist: {File.Exists(FilePath)}");
            client.Close();
        }
        ~Client()
        {
            GC.Collect(2, GCCollectionMode.Forced);
        }
        public void GetSheet(HTTPHeaders head)
        {
            // link = C:\...
            // address = file.html
            try
            {
                bool IsFile = File.Exists(head.ReadlPath);
                string sheet_params = "";
                //bool IsFolder = Directory.Exists(link);
                //Console.WriteLine($"File link: {link} File: {IsFile} Folder: {IsFolder}");
                
                if (!IsFile)
                {
                    string html = " ";
                    string headers = $"HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length: {html.Length}\n\n{html}";
                    // OUTPUT HEADERS
                    byte[] data_headers = Encoding.UTF8.GetBytes(headers);
                    client.GetStream().Write(data_headers, 0, data_headers.Length);
                }
                if (IsFile && GetFormat(head.ReadlPath) == "py" || GetFormat(head.ReadlPath) == "php")
                {
                    string html = AnyFile(head);
                    string content_type = GetContentType(head);
                    int length = html.Length;
                    string headers1 = "";
                    string headers = $"HTTP/1.1 200 OK\nContent-type: {content_type}\nContent-Length: {length}\n\n{headers1}{html}";
                    // OUTPUT HEADERS
                    byte[] data_headers = Encoding.UTF8.GetBytes(headers);
                    client.GetStream().Write(data_headers, 0, data_headers.Length);
                    IsFile = false;
                }
                if (IsFile)
                {
                    string content_type = GetContentType(head);
                    FileStream fs = new FileStream(head.ReadlPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    string headers = "";
                    headers = $"HTTP/1.1 200 OK\nContent-type: {content_type}\nContent-Length: {fs.Length}\n\n";
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
                    //client.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Func: GetSheet()    link: {head.ReadlPath}\nException: {ex}/nMessage: {ex.Message}");
            }
        }
        //TODO CGI
        string AnyFile(HTTPHeaders head)
        {
            string interpretator = "";
            string result = "";
            string ext = head.ReadlPath.Substring(head.ReadlPath.LastIndexOf("."))
                .Replace(".","")
                .Replace(" ", "");
            lock(new object())
            {
                foreach (var i in server.global.Interpreters)
                {
                    if (i.Value.Name == ext)
                            interpretator = $"{AppDomain.CurrentDomain.BaseDirectory}{i.Value.Path}";
                    if (i.Value.Name == ext && i.Value.Type == "cgi" && head.QueryString.Length == 0 || head.QueryString != "" || head.QueryString != null )
                            interpretator = $"{AppDomain.CurrentDomain.BaseDirectory}{i.Value.Path}";
                }
                Console.WriteLine(interpretator);
                result = interpreter.UseInterpreter(interpretator, head.ReadlPath);
            }
            return result;
        }
        string GetContentType(HTTPHeaders head)
        {
            string result = "";
            if(File.Exists(head.ReadlPath))
            {
                string format = head.ReadlPath.Substring(head.ReadlPath.LastIndexOf("."));
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
        string GetFormat(string link)
        {
            string result = "";
            if (File.Exists(link))
            {
                string format = link.Substring(link.LastIndexOf("."));
                result = format.Replace(".", "").ToLower();
                //Console.WriteLine($"link: {link} format: {format} content-type: {result}");
            }
            else
            {
                result = $"unknown";
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
