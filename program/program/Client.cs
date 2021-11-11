using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace program
{
    class Client
    {
        TcpClient client;
        //string site = @"D:\Моё портфолио\csharp_server\www";
        string site = @"C:\Users\Admin\Desktop\csharp_server\www";
        public Client(TcpClient c)
        {
            this.client = c;
            string FilePath = $"{site}";
            NetworkStream stream = client.GetStream();
            string request = "";
            byte[] data = new byte[1024];
            //Console.WriteLine($"Client ip: {client.Client.RemoteEndPoint}");
            while (stream.DataAvailable)
            {
                stream.Read(data, 0, data.Length);
                request += Encoding.UTF8.GetString(data);
            }
            // берет первую строку заголовков
            Match ReqMatch = Regex.Match(request, @"^\w+\s+([^\s\?]+)[^\s]*\s+HTTP/.*|");
            if (ReqMatch == Match.Empty)
            {
                SendError(400);
                return;
            }
            string file = ReqMatch.ToString();
            Console.WriteLine(ReqMatch);
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
            if (file == "/" || file == "\\")
            {
                file = "/index.html";
            }
            if (file.IndexOf("..") != -1)
            {
                SendError(400);
                return;
            }
            FilePath += $"{file}";
            GetSheet(FilePath);
            //Console.WriteLine($"Path: {FilePath} - Exist: {File.Exists(FilePath)}");
            client.Close();
        }
        public void SendError(int code)
        {
            Console.WriteLine($"Error {code}");
            string html = $"<html><head><title></title></head><body><h1>Error {code}</h1></body></html>";
            string headers = $"HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length: {html.Length}\n\n{html}";
            byte[] data = Encoding.UTF8.GetBytes(headers);
            client.GetStream().Write(data, 0, data.Length);
            client.Close();
        }
        public void SendError(string message, int code)
        {
            Console.WriteLine($"{client.Client.RemoteEndPoint} error: {code} message: {message}");
            string html = $"<html><head><title></title></head><body><h1>Error {code}</h1><div>{message}</div></body></html>";
            string headers = $"HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length: {html.Length}\n\n{html}";
            byte[] data = Encoding.UTF8.GetBytes(headers);
            client.GetStream().Write(data, 0, data.Length);
            client.Close();
        }
        public void GetSheet(string link)
        {
            try
            {
                Console.WriteLine($"File link: {link} Exist: {File.Exists(link)}");
                if (!File.Exists(link))
                {
                    SendError(400);
                    return;
                }
                string html = File.ReadAllText(link);
                string content_type = GetContentType(link);
                //FileExplorer(link);
                FileStream fs = new FileStream(link, FileMode.Open, FileAccess.Read, FileShare.Read);
                string headers = $"HTTP/1.1 200 OK\nContent-type: {content_type}\nContent-Length: {fs.Length}\n\n";
                //Console.WriteLine($"{headers}{link}");
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

                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Func: GetSheet()    link: {link}\nException: {ex}/nMessage: {ex.Message}");
            }
        }
        bool FileExplorer(string link)
        {
            string format = link.Substring(link.LastIndexOf("."));
            format = format.Replace(".", "").ToLower();
            Console.WriteLine($"{format} - {link}");
            if (format.Length == 0)
            {
                Console.WriteLine("Length 0");
            }
            return false;
        }
        string GetHeaders(string content_type)
        {
            string result = "";

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
                    case "php":
                    case "markdown":
                    case "md":
                        result = $"text/{format}";
                        break;
                    case "javascript":
                    case "js":
                        result = $"text/javascript";
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
        void Close()
        {
            client.Close();
        }
    }
}
