using System;
using System.Collections.Generic;

namespace AMES
{
    internal class Http
    {

        public static HttpRequestType ParseRequestType(string requestType)
        {
            HttpRequestType result = HttpRequestType.NONE;

            switch(requestType)
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


        public static Dictionary<string, string> Parse(ref string headers)
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

                    if(link.IndexOf('?') != -1)
                    {
                        string data = GetData(link);
                        link = link.Replace(data, "");

                        result.Add("DATA", data);
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
            headers = null;
            return result;
        }

        private static string GetData(string headers)
        {
            string result = "";
            bool read = false;

            for(int i = 0; i < headers.Length; i++)
            {
                if(read)
                {
                    result += headers[i];
                }
                else
                {
                    read = headers[i] == '?' ? true : false;
                }
            }

            return result;
        }

        private static Dictionary<string, string> ParseLines(string headers, int count = 0)
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