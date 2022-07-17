using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AMES
{
    internal class Http
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
            int count = 0;
            string line = "";
            string lineName = "";
            bool hasName = false;

            for(int i = 0; i < headers.Length; i++)
            {
                if(headers[i] == '\n')
                {
                    if(!hasName && count == 0)
                    {
                        HttpRequestType type = GetRequestType(lineName);
                        lineName = lineName.Replace($"{type}", "");
                        string temp = "";
                        for(int j = lineName.Length - 1; j != 0; j--)
                        {
                            if(lineName[j] == ' ')
                            {
                                break;
                            }  
                            temp += lineName[j]; 
                        }
                        lineName = lineName.Replace(temp, "");
                        line = lineName.Replace(" ", "");
                        lineName = "IOAddress";
                    }
                    result.Add(lineName, line);
                    lineName = ""; 
                    line = "";
                    count++;
                    Console.WriteLine(lineName + ":" + line);
                    continue;
                }

                if(!hasName)
                {
                    if(headers[i] == ':')
                    {
                        hasName = true;
                    }
                    lineName += headers[i];
                }
                else
                {
                    line += headers[i];
                }
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