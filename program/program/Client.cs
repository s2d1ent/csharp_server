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
    class Client
    {
        TcpClient client;
        Server server;
        string site = @"";
        Interpreter interpreter = new Interpreter();
        string params_request = "";
        List<string> Temp = new List<string>();
        bool django = false;
        //string site = @"C:\Users\Admin\Desktop\csharp_server\www";
        public Client(TcpClient c, Server s)
        {
            this.client = c;
            this.server = s;
            site = server.Path;
            string FilePath = $"{AppDomain.CurrentDomain.BaseDirectory}{site}";
            NetworkStream stream = client.GetStream();
            string request = "";
            byte[] data = new byte[1024];
            string get_post_data = "";
            //Console.WriteLine($"Client ip: {client.Client.RemoteEndPoint}");
            while (stream.DataAvailable)
            {
                stream.Read(data, 0, data.Length);
                request += Encoding.UTF8.GetString(data);
            }
            
            // берет первую строку заголовков
            Match ReqMatch = Regex.Match(request, @"^\w+\s+([^\s\?]+)[^\s]*\s+HTTP/.*|");
            string domain = Regex.Match(request, @"Host:\s[a-z]+").Value.Replace("Host:", "").Replace(" ", "");
            if (ReqMatch == Match.Empty)
            {
                Console.WriteLine($"ReqMatch = Empty");
                SendError(400);
                return;
            }
            Console.WriteLine(request);
            string file = ReqMatch.ToString();
            foreach(var i in server.global.Alias)
                if(i.Value == domain)
                    domain = i.Key;
            /*if(domain == "" || domain.Length == 0)
            {
                Console.WriteLine($"domain = null");
                SendError(404);
                return;
            }*/
            if(file.Length != 0)
            {
                file = file.Replace("GET", "")
                .Replace("POST", "")
                .Replace(
                    file.Substring(
                        file.IndexOf("HTTP")
                        ), "")
                .Replace(" ", "");
            }
            if(file.IndexOf("?")!=-1)
            {
                Console.WriteLine(file);
                this.params_request = file.Substring(file.IndexOf("?"));
                file = file.Replace(file.Substring(file.IndexOf("?")),"");
            }
            if (file.Length == 0)
            {
                client.Close();
                return;
            }
            if (file == "/" || file == "\\" || file == " " || file == "")
            {
                foreach(var ext in server.Extensions)
                {
                    if (File.Exists($"{site}/{domain}/index{ext}"))
                    {
                        Console.WriteLine($"/index{ext}");
                        file = $"/index{ext}";
                        break;
                    }
                }
                if(File.Exists($"{site}/{domain}/manage.py"))
                {
                    file = $"/manage.py";
                    django = true;
                }
            }
            if (file.IndexOf("..") != -1)
            {
                client.Close();
                return;
            }
            FilePath += $"/{domain}{file}";
            GetSheet(FilePath, file);
            //Console.WriteLine($"Path: {FilePath} - Exist: {File.Exists(FilePath)}");
            client.Close();
        }
        ~Client()
        {
            GC.Collect(2, GCCollectionMode.Forced);
        }
        public void GetSheet(string link, string file)
        {
            // link = C:\...
            // address = file.html
            try
            {
                bool IsFile = File.Exists(link);
                //bool IsFolder = Directory.Exists(link);
                //Console.WriteLine($"File link: {link} File: {IsFile} Folder: {IsFolder}");
                if (!IsFile)
                {
                    string html = " ";
                    string headers = $"HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length: {html.Length}\n\n{html}";
                    // OUTPUT HEADERS
                    byte[] data_headers = Encoding.UTF8.GetBytes(headers);
                    client.GetStream().Write(data_headers, 0, data_headers.Length);
                    // OUTPUT CONTENT
                    byte[] data = Encoding.UTF8.GetBytes(html);
                    client.GetStream().Write(data, 0, data.Length);
                }
                if (IsFile && GetFormat(link) == "py")
                {
                    string html = AnyFile(link);
                    string content_type = GetContentType(link);
                    string headers = $"HTTP/1.1 200 OK\nContent-type: {content_type}\nContent-Length: {html.Length}\n\n{html}";
                    // OUTPUT HEADERS
                    byte[] data_headers = Encoding.UTF8.GetBytes(headers);
                    client.GetStream().Write(data_headers, 0, data_headers.Length);
                    // OUTPUT CONTENT
                    byte[] data = Encoding.UTF8.GetBytes(html);
                    client.GetStream().Write(data, 0, data.Length);
                    IsFile = false;
                }
                if (IsFile && GetFormat(link) == "php")
                {
                    string html = AnyFile(link);
                    string content_type = GetContentType(link);
                    Console.WriteLine(html);
                    string headers = $"HTTP/1.1 200 OK\nContent-type: {content_type}\nContent-Length: {html.Length}\n\n";
                    // OUTPUT HEADERS
                    byte[] data_headers = Encoding.UTF8.GetBytes(headers);
                    client.GetStream().Write(data_headers, 0, data_headers.Length);
                    // OUTPUT CONTENT
                    byte[] data = Encoding.UTF8.GetBytes(html);
                    client.GetStream().Write(data, 0, data.Length);
                    /*string temp = "";
                    for(var i = 0; i < 8; i++)
                    {
                        byte rnd = (byte)new Random().Next(66, 90);
                        temp += Encoding.UTF8.GetString(new byte[]{ rnd });
                    }
                    temp += ".html";
                    string temp_link = $"{AppDomain.CurrentDomain.BaseDirectory}/temp/{temp}";
                    File.WriteAllText(temp_link, html);
                    GetSheet(temp_link, temp);
                    IsFile = false;*/
                }
                if (IsFile)
                {
                    string content_type = GetContentType(link);
                    FileStream fs = new FileStream(link, FileMode.Open, FileAccess.Read, FileShare.Read);
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
                Console.WriteLine($"Func: GetSheet()    link: {link}\nException: {ex}/nMessage: {ex.Message}");
            }
        }
        string AnyFile(string address)
        {
            string interpretator = "";
            string result = "";
            string ext = address.Substring(address.LastIndexOf("."))
                .Replace(".","")
                .Replace(" ", "");
            lock(new object())
            {
                foreach (var i in server.global.Interpreters)
                {
                    if (i.Value.Name == ext)
                        if (i.Value.Version == server.global.System["Bit"])
                            interpretator = $"{AppDomain.CurrentDomain.BaseDirectory}{i.Value.Path}";
                }
                //string result = php.PerformPhp(interpretator , $"{address}");
                //Console.WriteLine(interpretator);
                result = interpreter.UseInterpreter(interpretator, address);
            }
            return result;
        }
        string GetContentType(string link)
        {
            string result = "";
            if(File.Exists(link))
            {
                string format = link.Substring(link.LastIndexOf("."));
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
                        result = $"{format}";
                        break;
                    case "EDI":
                    case "EDI-X12":
                    case "EDIFACT":
                        result = $"{format}"; break;
                    case "atom":
                        result = $"{format}"; break;
                    case "soap":
                        result = $"{format}"; break;
                    case "woff":
                        result = $"{format}"; break;
                    case "xhtml":
                        result = $"{format}"; break;
                    case "torrent":
                    case "bittorrent":
                        result = $"{format}"; break;
                    case "tex":
                        result = $"{format}"; break;
                    //audio
                    case "basic":
                    case "l24":
                    case "mp4":
                    case "acc":
                    case "mpeg":
                    case "vorbis":
                    case "webm":
                        result = $"{format}"; break;
                    case "wma":
                        result = $"{format}"; break;
                    case "rm":
                        result = $"{format}"; break;
                    case "wav":
                    case "wave":
                        result = $"{format}"; break;
                    //image
                    case "gif":
                    case "jpeg":
                    case "pjpeg":
                    case "png":
                    case "tiff":
                    case "webp":
                        result = $"{format}"; break;
                    case "svg":
                        result = $"svg+xml";
                        break;
                    case "ico":
                        result = $"vnd.microsoft.icon";
                        break;
                    case "wbmp":
                        result = $"vnd.map.wbmp";
                        break;
                    case "jpg":
                        result = $"jpeg";
                        break;
                    //message
                    case "eml":
                    case "mime":
                    case "mth":
                    case "mhtml":
                        result = $"ftc822";
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
                        result = $"{format}"; break;
                    case "x3db":
                        result = $"x3d+binary";
                        break;
                    case "x3dv":
                        result = $"x3d+vrml";
                        break;
                    case "x3d":
                        result = $"x3d+xml";
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
                        result = $"{format}"; break;
                    case "javascript":
                    case "js":
                        result = $"{format}"; break;
                    case "php":
                        result = $"{format}"; break;
                    case "py":
                        result = $"{format}"; break;
                    //video
                    //vnd
                    //x
                    //x-pkcs
                    default:
                        result = $"{format}"; break;
                }
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
