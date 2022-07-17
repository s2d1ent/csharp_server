using System;
using System.Collections;
using System.Collections.Generic;

namespace Program
{
    class Program
    {
        static void main()
        {
            string headers = @"POST /php/auth.php?name=admin&passwd=admin HTTP/1.1
Host: uppdd
Connection: keep-alive
Content-Length: 62
Cache-Control: max-age=0
Upgrade-Insecure-Requests: 1
Origin: http://uppdd
Content-Type: application/x-www-form-urlencoded
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.85 YaBrowser/21.11.1.212 Yowser/2.5 Safari/537.36
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9
Referer: http://uppdd/signin.php
Accept-Encoding: gzip, deflate
Accept-Language: ru,en;q=0.9,la;q=0.8

{
	'Ipv4': null,
	'Port' : 80,
	'Server':{
		'Modules': {}
	},
	'EnabledServerModules': false,
	'Multiple': false,
	'Php':'/bin/',
	'Python':null,
	'Alias': {
		'python':'kidalovo'
	},
	'MinWork': 2,
	'MinWorkAsync': 2,
	'MaxWork': 2,
	'MaxWorkAsync': 2
}";
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
            
            foreach(var elem in lines)
            {
                string key = "";
                string value = elem.Value;
                if(count == 0)
                {
                    HttpRequestType type = Http.GetRequestType(value);
                    string IOAddress = "";
                    value = value.Replace($"{type}", "").TrimStart();

                    for(int i = 0; i < value.Length; i++)
                    {
                        if(value[i] == ' ')
                        {
                            break;
                        }
                        IOAddress += value[i];  
                    }
                    
                    value = value.Replace(IOAddress, "").Replace(" ", "");

                    result.Add("HTTP_REQUEST_TYPE", type.ToString());
                    result.Add("HTTP_FILE_PATH", IOAddress);
                    result.Add("HTTP_VERSION", value);
                } 
                else if(value[0] == '{' && value[value.Length - 1] == '}')
                {
                   result["DATA"] = value;     
                   continue;
                }
                else
                {
                    string tempValue = "";
                    bool hasKey = false;

                    for(int i = 0; i < value.Length; i++)
                    {
                        if(value[i] == ':')
                        {
                            hasKey = true;
                            continue;
                        }
                        if(!hasKey)
                        {
                            key += value[i];
                        }
                        else
                        {
                            tempValue += value[i];
                        }
                    }    

                    if(key.Length != 0 && tempValue.Length != 0)
                    {
                        result.Add(key, tempValue);
                    }
                    else
                    {
                        result.Add("DATA", key);
                    }
                }   
                count++;
            }
            count--;

            if(result["HTTP_REQUEST_TYPE"] == "POST")
            {
                string data = ParseDataPost(
                    result["HTTP_FILE_PATH"]
                );
                result["DATA"] = data;   
            }
            else if(result["HTTP_REQUEST_TYPE"] == "GET")
            {
                string data = ParseDataGet(
                    result["HTTP_FILE_PATH"]
                );
                result["DATA"] = data;
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

        private static string ParseDataPost(string line)
        {
            string result = "";

            return result;
        }

        private static Dictionary<string, string> ParseLines(string headers, int count)
        {
            Dictionary<string, string> result = new();
            string line = "";
            int i = 0;
            
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
                    break;
                }
                if(headers[i] == '{' && 
                    headers[headers.Length - 1] == '}')
                {
                    line = headers;
                    break;
                }
                
                line += headers[i];
                i++;
            }
            result.Add($"lines_{count}", line);
            headers = headers.Replace(line, "");
            Dictionary<string, string> tempParse = new();
            
            if(headers.Length != 0)
            {
                tempParse = ParseLines(headers, ++count);
            }
            
            foreach(KeyValuePair<string, string> elem in tempParse)
            {
                result.Add(elem.Key, elem.Value);
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
