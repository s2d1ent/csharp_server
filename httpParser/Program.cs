using System;
using System.Collections;
using System.Collections.Generic;

namespace Program
{
    class Program
    {
        static void main()
        {
            string headers = @"PUT /index.htm HTTP/1.1
Host: viktor
Content-Type: text/plain
Content-Length: 83

<!DOCTYPE html>
<html>
<head>
</head>
<body>
<h1>Hello, World!</h1>
</body>
</html>";
            Dictionary<string, string> headersParsed = new();
            headersParsed = Http.Parse(headers);

            foreach(var elem in headersParsed)
            {
                Console.WriteLine($"{elem.Key} : {elem.Value}");
            }
        }
    }

    class Http
    {
        public static HttpRequestType GetRequestType(string header)
        {
            HttpRequestType result = HttpRequestType.NONE;
            string type = "";
            int i = 0;

            while(header[i] != ' ')
            {
                type += header[i];
                i++;
            }
            
            switch(type)
            {
                case "OPTIONS":
                    result =HttpRequestType.OPTIONS;
                break;
                case "GET":
                    result =HttpRequestType.GET;
                break;
                case "HEAD":
                    result =HttpRequestType.HEAD;
                break;
                case "POST":
                    result =HttpRequestType.POST;
                break;
                case "PUT":
                    result =HttpRequestType.PUT;
                break;
                case "PATCH":
                    result =HttpRequestType.PATCH;
                break;
                case "DELETE":
                    result =HttpRequestType.DELETE;
                break;
                case "TRACE":
                    result =HttpRequestType.TRACE;
                break;
                case "CONNECT":
                    result =HttpRequestType.CONNECT;
                break;
                default:
                    result = HttpRequestType.NONE;
                break;
            }
            
            return result;
        }

        public static Dictionary<string, string> Parse(string headers)
        {
            Dictionary<string, string> result = new();
            Dictionary<string, string> lines = ParseLines(headers, 0);
            int count = 0;
            
            foreach(KeyValuePair<string, string> elem in lines)
            {
                string key = elem.Key;
                string value = elem.Value.Replace("\n", "");
                if(count == 0)
                {
                    // parse first line in headers
                    // get path, http version and request type
                    string type = GetRequestType(value).ToString();
                    string link = "", httpType = "";
                    value = value.Replace(type, "").TrimStart();

                    for(int i = 0 ; i < value.Length; i++)
                    {
                        if(value[i] == ' ')
                        {
                            break;
                        }
                        link += value[i];
                    }
                    value = value.Replace(link, "").TrimStart();

                    result.Add("HTTP_REQUEST_TYPE", type);
                    result.Add("HTTP_LINK", link);
                    result.Add("HTTP_VERSION", value);
                    count++;
                }
                else
                {
                    // other lines in headers
                    if(key == "DATA")
                    {
                        result.Add(key, value);
                        continue;
                    }

                    key = "";

                    for(int i = 0; i < value.Length; i++)
                    {
                        if(value[i] == ':')
                        {
                            break;
                        }
                        key += value[i];
                    }
                    value = value.Replace(key, "")
                        .TrimStart(':')
                        .TrimStart();

                    result.Add(key, value);
                }
            }

            return result;
        }

        private static string ParseDataGet(string line)
        {
            string result = "";
            int index = 0;
   
            index = line.IndexOf('?');
            
            if(index == -1)
            {
                return result;
            }
            
            for(int i = ++index; i < line.Length; i++)
            {
                result += line[i];
            }

            return result;
        }

        private static string ParseDataPost(string headers)
        {
            string result = "";

            return result;
        }

        public static Dictionary<string, string> ParseLines(string headers, int count = 0)
        {
            Dictionary<string, string> result = new();
            string line = "";
            int i = 0;
            bool segmentData = false;
            
            if(headers == null || headers.Length == 0 || headers.Length == 1)
            {
                return result;
            }
            while(headers[0] == '\n')
            {
                headers = headers.TrimStart();
            }

            while(true)
            {
                if(i == headers.Length - 1)
                {
                    break;
                }
                // TODO 
                if(headers[i] == '\n')
                {
                    line += headers[i];
                    break;
                }
                line += headers[i];
                i++;
            }

            if (line.Length == 2)
            {
                headers = headers.Replace(line, "");
                result.Add("DATA", headers);
                segmentData = true;
            }
            
            result.Add($"lines_{count}", line);
            headers = headers.Replace(line, "");
            Dictionary<string, string> tempParse = new();
            
            if(headers.Length != 0 && !segmentData)
            {
                tempParse = ParseLines(headers, ++count);
            }
            
            foreach(KeyValuePair<string, string> elem in tempParse)
            {
                result.Add(elem.Key, 
                    elem.Value.Replace("\n", "")
                );
            }
            return result;
        }

        internal enum HttpRequestType
        {
            OPTIONS,
            GET,
            HEAD,
            POST,
            PUT,
            PATCH,
            DELETE,
            TRACE,
            CONNECT,
            NONE
        }
    }
}
