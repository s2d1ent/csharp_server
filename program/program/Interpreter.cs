using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
//using Newtonsoft.Json;

namespace program
{
    class Interpreter
    {
        public string Version { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        [JsonIgnore]
        public string Reponse { get; set; }
        [JsonIgnore]
        public bool Error;
        [JsonIgnore]
        public string Error_message;
        public Interpreter() 
        {
            //this.Path = @"D:\csharp_server\program\program\bin\Debug\net5.0\includes\php\win64\php.exe";
        }
        public string PerformPhp(string php, string file)
        {
            ProcessStartInfo info = new ProcessStartInfo(php, file);
            info.UseShellExecute = false;
            info.ErrorDialog = false;
            info.RedirectStandardError = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.CreateNoWindow = true;


            Process p = new Process();
            p.StartInfo = info;

            bool pStarted = p.Start();

            StreamWriter input = p.StandardInput;
            StreamReader output = p.StandardOutput;
            StreamReader error = p.StandardError;


            return output.ReadToEnd();
        }
    }
}
