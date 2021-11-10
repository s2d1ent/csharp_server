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
        string site = @"C:\Users\Admin\Desktop\csharp_server\www";
        public Client(TcpClient client)
        {
            this.client = client;
            string FilePath = $"{site}";
            NetworkStream stream = client.GetStream();
            string request = "";
            string response = "";
            byte[] data = new byte[1024];
            byte[] request_bye = new byte[1024];
            Console.WriteLine($"Client ip: {client.Client.RemoteEndPoint}");
            while (stream.DataAvailable)
            {
                stream.Read(data, 0, data.Length);
                request += Encoding.UTF8.GetString(data);
            }

            Match ReqMatch = Regex.Match(request, @"^\w+\s+([^\s\?]+)[^\s]*\s+HTTP/.*|");

            if (ReqMatch == Match.Empty)
            {
                SendError(400);
                return;
            }

            string RequestUri = ReqMatch.Groups[1].Value;
            RequestUri = Uri.UnescapeDataString(RequestUri);
            FilePath += $"/{RequestUri}";
            if (RequestUri.IndexOf("..") >= 0)
            {
                SendError(400);
                return;
            }
            if (RequestUri.EndsWith("/"))
            {
                RequestUri += "index.html";
                FilePath += $"/{RequestUri}";
            }
            if(!File.Exists(FilePath))
            {
                SendError(400);
                return;
            }
            else
            {
/*                string Extension = RequestUri.Replace("GET", "")
                .Replace("POST", "")
                .Replace("HTTP1/1", "");
                Extension = Extension.Substring(Extension.IndexOf("."));*/
                string ContentType = "";
                if(RequestUri.IndexOf(".html") != -1 || RequestUri.IndexOf(".htm") != -1)
                    ContentType = "text/html";
                else if (RequestUri.IndexOf(".css") != -1)
                    ContentType = "text/stylesheet";
                else if (RequestUri.IndexOf(".js") != -1)
                    ContentType = "text/javascript";
                else
                {
                    ContentType = $"application/{RequestUri.Substring(RequestUri.IndexOf("."))}";
                }
                string html_request = "";
                try
                {
                    html_request = File.ReadAllText(FilePath);
                }
                catch
                {
                    SendError(404);
                    return;
                }
                string headers = $"HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length: {html_request.Length}\n\n{html_request}";
                Console.WriteLine(headers);
                while (stream.DataAvailable)
                {
                    stream.Write(request_bye, 0, request_bye.Length);
                }
            }
            

            client.Close();
        }
        public void SendError(int code)
        {
            string html = $"<html><body><h1>Error {code}</h1></body></html>";
            string headers = $"HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length: {html.Length}\n\n{html}";
            byte[] data = new byte[1024];
            while (client.GetStream().DataAvailable)
            {
                data = Encoding.UTF8.GetBytes(headers);
                client.GetStream().Write(data, 0, data.Length);
            }
        }
    }
}
