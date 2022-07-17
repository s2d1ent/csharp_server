using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMES
{
    internal class Php : Interpreter
    {
        public byte[] GetFile(string path)
        {
            byte[] result = new byte[1024];
            string err = "";
            string data = "";
            
            ProcessStartInfo info = new ProcessStartInfo(Constants.PATH_PHP, path);
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

            if(output.CurrentEncoding != Encoding.UTF8)
            {
                err = error.ReadToEnd();
                data = output.ReadToEnd();
                err += data;
                byte[] buffer = output.CurrentEncoding.GetBytes(err);
                result = Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, buffer);

            }
            else
            {
                err = error.ReadToEnd();
                data = output.ReadToEnd();
                err += data;
                result = output.CurrentEncoding.GetBytes(err);  
            }

            return result;
        }
    }
}
