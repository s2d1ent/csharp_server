using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace AMES
{
    internal class Client
    {
        public Dictionary<string, string> Headers;
        public Configurator Configurator {get;set;}
        private Socket _client;
        private string _headers;
        private Html _html = new();
        private Php _php { get; set; }
        private Python _python { get; set; }
        private AMESLogger _logger;

        public Client(Socket socket, Configurator configurator)
        {
            Configurator = configurator;
            _client = socket;
            _client.Ttl = 30;
            _logger = new();
            _php = Constants.PATH_PHP == null || Constants.PATH_PHP == "" ? null : new Php();
            _python = Constants.PATH_PYTHON == null || Constants.PATH_PYTHON == "" ? null : new Python();
        }

        // Start work with client request 
        public void Start()
        {
            string path = "";
            string fileExtensions = "";
            byte[] buffer = new byte[_client.ReceiveBufferSize];
            HttpRequestType requestType = HttpRequestType.NONE;

            try
            {
                // get headers
                _client.Receive(buffer);
                _headers = Encoding.UTF8.GetString(buffer);

                Headers = Http.Parse(ref _headers);

                foreach(KeyValuePair<string, string> elem in Headers)
                {
                    //Console.WriteLine($"{elem.Key} - {elem.Value}");
                }

                // get path
                path = Headers["HTTP_LINK"];

                // checked headers
                if (path.IndexOf("..") != -1)
                {
                    Error(HttpCodes.BadRequest);
                    _client.Close();
                    return;
                }

                requestType = Http.ParseRequestType(Headers["HTTP_REQUEST_TYPE"]);

                // If path equal to /? that choice index file
                if(path == @"\" || path == "/")
                {
                    if(Configurator.ServerMode == ServerMode.Multiple)
                    {

                    }   
                    else
                    {
                        for(int i = 0; i < Constants.EXTENSIONS.Length; i++)
                        {
                            string value = Constants.EXTENSIONS[i];
                            string find = Constants.ROOT;

                            if(value == "")
                            {
                                break;
                            }

                            if(value[0] == '.')
                            {
                                find += "index" + value;
                            }
                            else
                            {
                                find += "index." + value;
                            }

                            if(File.Exists(find))
                            {
                                
                                path = find;
                                if(value.IndexOf("php") != -1 && _php == null)
                                {
                                    continue;
                                }
                                else if(value.IndexOf("py") != -1 && _python == null)
                                {
                                    continue;
                                }
                                fileExtensions = value;
                                break;
                            }
                        }
                    }
                    
                }
                else
                {
                    path = Constants.ROOT + path;
                    path = path.Replace(@"\\", @"\").Replace(@"//", @"//");
                }

                switch(requestType)
                {
                    case HttpRequestType.GET:
                        string requestHeaders = "";
                        bool hasCache = false;
                        byte[] html = new byte[0];
                        string contentType = GetContentType(path);
                        path = path.Replace(@"\\", @"\").Replace(@"//", @"//");

                        if(!File.Exists(path))
                        {
                            Error(HttpCodes.BadRequest);
                            break;
                        }

                        if(!hasCache)
                        {
                            if(fileExtensions.IndexOf("php") != -1 && _php != null)
                            {
                                html = _php.GetFile(path);    
                            }
                            else
                            {
                                html = _html.GetFile(path);
                            }
                        }
                        else
                        {
                            html = Configurator.Cache.GetPage(path);
                        }

                        requestHeaders = "HTTP/1.1 200 OK\n"+
                        $"Content-Type: {contentType}\n" +
                        $"Content-Length: {html.Length}\n\n";

                        buffer = Encoding.UTF8.GetBytes(requestHeaders);

                        _client.Send(
                            buffer, SocketFlags.None
                        );
                         _client.Send(
                            html, SocketFlags.None
                        );
                    break;
                    case HttpRequestType.DELETE:
                        HttpCodes requestDelete = Interpreter.Delete(path);
                        HttpDelete(requestDelete);
                    break;
                    case HttpRequestType.OPTIONS:
                        HttpOptions();
                    break;
                    case HttpRequestType.TRACE:
                        HttpTrace(_headers);    
                    break;
                    case HttpRequestType.PUT:
                        HttpPut(path);    
                    break;
                    default:
                        Error(HttpCodes.BadRequest);
                    break;
                }

            }
            catch(Exception ex)
            {
                Error(HttpCodes.InternalServer);
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _client.Close();

                // cache add page
                Configurator.Cache.Add(path);
            }
        }

        // Return content-type
        private string GetContentType(string path)
        {
            string result = "";

            try
            {
              string format = path.Substring(path.LastIndexOf("."));
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
                 case "htm":
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
            }
            catch
            {
             string log = "Function \"GetContentType\" catched exception";
                 Error(HttpCodes.InternalServer);
                 _logger.SetLog(AMESModuleType.Client, log);
                 return "application/unknown"; 
            }

            return result;
        }

        // Sends error to client
        private void Error(HttpCodes httpCodes)
        {
            byte[] buffer;
            string html = $"<h1>Error {(int) httpCodes}</h1></br><h2>{httpCodes}</h2>";
            string headers = $"HTTP/1.1 {(int)httpCodes} \nContent-Type: text/html\nContent-Length: {html.Length}\n\n{html}";

            buffer = Encoding.UTF8.GetBytes(headers);
            _client.Send(buffer, SocketFlags.None);
        }

        // TODO FILE DATA
        // HANG UP CLIENT
        // HTTP PUT request
        private void HttpPut(string path)
        {
            try
            {
                path = path.Replace(@"//", @"/");
                HttpCodes code = HttpCodes.NONE;
                byte[] buffer = new byte[0];
                string headers = "";

                if(!File.Exists(path))
                {
                    code = HttpCodes.Created;
                    File.Create(path);
                    if(Headers.TryGetValue("DATA", out string val))
                    {
                        File.WriteAllText(
                            path, 
                            Headers["DATA"]
                        );
                    }
                }
                else
                {
                    if(Headers.TryGetValue("DATA", out string val))
                    {
                        File.WriteAllText(
                            path, 
                            Headers["DATA"]
                        );
                    }
                    code = HttpCodes.OK;
                }

                headers = $"HTTP/1.1 {(int)code} {code}\n"+
                $"Content-Location: {path}\n\n";

                buffer = Encoding.UTF8.GetBytes(headers);
                _client.Send(buffer);
            }
            catch
            {
                string log = $"Client: {_client.RemoteEndPoint} HTTP: PUT error";
                Error(HttpCodes.InternalServer);
                 _logger.SetLog(AMESModuleType.Client, log); 
            }
        }

        // HTTP TRACE request
        private void HttpTrace(string traceHeaders)
        {
            try
            {
                byte[] buffer;
                string headers = $"HTTP/1.1 {(int)HttpCodes.OK} {HttpCodes.OK}\n"+
                $"Server:{Constants.NAME}/{Constants.VERSION} ({Constants.OS})\n"+
                $"Content-Type: message/http\n"+
                $"Content-Length: {traceHeaders.Length}\n\n{traceHeaders}";

                buffer = Encoding.UTF8.GetBytes(headers);
                _client.Send(buffer, SocketFlags.None); 
            }  
            catch(Exception ex)
            {
                string catchLog = $"Error catch HttpTrace {ex.Message}";
                _logger.SetLog(AMESModuleType.Client, catchLog);
                Error(HttpCodes.InternalServer);
                _client.Close();    
            }
            finally
            {
                string log = $"Client: {_client.RemoteEndPoint} HTTP: TRACE";
                _logger.SetLog(AMESModuleType.Client, log);
            }
        }

        // HTTP DELETE request
        private void HttpDelete(HttpCodes httpCodes)
        {
            try
            {
                if(httpCodes == HttpCodes.InternalServer)
                {
                    Error(HttpCodes.InternalServer);
                    return;   
                } 
                if(httpCodes == HttpCodes.NoContent)
                {
                    byte[] buffer;
                    string headers = $"HTTP/1.1 {(int)httpCodes} No Content\n"+
                    $"Content-Type: text/plain\n"+
                    $"Content-Length: 0\n\n";
                    Console.WriteLine(headers);
                    buffer = Encoding.UTF8.GetBytes(headers);
                    _client.Send(buffer, SocketFlags.None);
                }  
                if(httpCodes == HttpCodes.NotFound)
                {
                    Error(HttpCodes.NotFound);
                    return;   
                } 
            }
            catch(Exception ex)
            {
                string catchLog = $"Error catch HttpDelete {ex.Message}";
                _logger.SetLog(AMESModuleType.Client, catchLog);
                Error(HttpCodes.InternalServer);
                _client.Close();    
            }
        }

        // HTTP OPTIONS request
        private void HttpOptions()
        {
            try
            {
                
                byte[] buffer;
                string headers = $"HTTP/1.1 {(int)HttpCodes.OK} {HttpCodes.OK}\n"+
                $"Allow: {Constants.ALLOW_HTTP_OPTIONS}\n"+
                $"Server:{Constants.NAME}/{Constants.VERSION} ({Constants.OS})\n"+
                $"Content-Type: text/plain\n"+
                $"Content-Length: 0\n\n";
                

                buffer = Encoding.UTF8.GetBytes(headers);
                _client.Send(buffer, SocketFlags.None);
                
            }
            catch(Exception ex)
            {
                string catchLog = $"Error catch OptionMessage {ex.Message}";
                _logger.SetLog(AMESModuleType.Client, catchLog);
                Error(HttpCodes.InternalServer);
                _client.Close();    
            }
        }


        // HTTP OPTIONS request with path
        private void HttpOptions(string path)
        {
            try
            {
                byte[] buffer;
                byte[] html = _html.GetFile(path);
                string contentType = "text/html";
                string headers = $"HTTP/1.1 200 OK\nAllow: {Constants.ALLOW_HTTP_OPTIONS}\n";
                headers+=$"Server: {Constants.NAME} {Constants.VERSION}\n";
                headers+=$"Content-Type: {contentType}\n";
                headers+=$"Content-Length: {html.Length}\n";

                buffer = Encoding.UTF8.GetBytes(headers);
                
                _client.Send(buffer, SocketFlags.None);
                _client.Send(html, SocketFlags.None);
            }
            catch(Exception ex)
            {
                string catchLog = $"Error catch OptionMessage {ex.Message}";
                _logger.SetLog(AMESModuleType.Client, catchLog);
                Error(HttpCodes.InternalServer);
                _client.Close();    
            }
        }
    }
    internal enum HttpCodes
    {
        NONE = 0,
        OK = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,
        BadRequest = 400,
        NotFound = 404,
        InternalServer = 500
        
    }

}