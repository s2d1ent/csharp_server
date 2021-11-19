using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
//using Newtonsoft.Json;

namespace program
{
    class Interpreter
    {
        public string Version { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Interpreter() 
        {
            //this.Path = @"D:\csharp_server\program\program\bin\Debug\net5.0\includes\php\win64\php.exe";
        }
        public string UseInterpreter(string php, string file)
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

            byte[] cp866_byte = output.CurrentEncoding.GetBytes(output.ReadToEnd());
            string cp866_byte_to_utf8 = Encoding.UTF8.GetString(cp866_byte);

            return cp866_byte_to_utf8;
        }
        public void OpenApplication(string path, string options)
        {
            ProcessStartInfo info = new ProcessStartInfo(path, options);
            info.UseShellExecute = false;
            info.ErrorDialog = false;
            info.RedirectStandardError = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.CreateNoWindow = true ;
            info.WindowStyle = ProcessWindowStyle.Normal;
            


            Process p = new Process();
            p.StartInfo = info;

            bool pStarted = p.Start();

            StreamWriter input = p.StandardInput;
            StreamReader output = p.StandardOutput;
            StreamReader error = p.StandardError;
        }
    }
}
