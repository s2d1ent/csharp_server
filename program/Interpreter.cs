using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Program
{
    class Interpreter
    {
        public string Version { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public Interpreter() {}
        public string UseInterpreter(string pathexe, string file)
        {
            ProcessStartInfo info = new ProcessStartInfo(pathexe, file);
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
        public static string UseCGI(HttpHeaders headers)
        {
            ProcessStartInfo info = new ProcessStartInfo(headers.InterpreterPath);
            string cp866_byte_to_utf8 = "";
            
            try
            {
                // параметры процесса
                info.UseShellExecute = false;
                info.ErrorDialog = false;
                info.RedirectStandardError = true;
                info.RedirectStandardInput = true;
                info.RedirectStandardOutput = true;
                info.CreateNoWindow = true;
                // переменны среды
                info.EnvironmentVariables.Add("REQUEST_METHOD", headers.Method);
                info.EnvironmentVariables.Add("REDIRECT_STATUS", headers.Redirect);
                info.EnvironmentVariables.Add("GETAWAY_INTERFACE", "CGI");
                info.EnvironmentVariables.Add("CONTENT_TYPE", headers.ContentType);
                info.EnvironmentVariables.Add("HTTP_ACCEPT", "*.*");
                info.EnvironmentVariables.Add("SCRIPT_FILENAME", headers.RealPath);
                if (headers.Method == "POST" || headers.Method == "PUT")
                    info.EnvironmentVariables.Add("CONTENT_LENGTH", headers.ContentLength);
                info.EnvironmentVariables.Add("QUERY_STRING", headers.QueryString);
                Process p = new Process();
                p.StartInfo = info;
                bool pStarted = p.Start();

                StreamWriter input = p.StandardInput;
                if (headers.Method == "POST" || headers.Method == "PUT")
                    input.WriteLine(headers.QueryString);
                StreamReader output = p.StandardOutput;
                StreamReader error = p.StandardError;

                byte[] cp866_byte = output.CurrentEncoding.GetBytes(output.ReadToEnd());
                cp866_byte_to_utf8 = Encoding.UTF8.GetString(cp866_byte);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"From: UseCGI {ex}\n{ex.Message}\n{ex.Data}");
            }
            return cp866_byte_to_utf8;
        }
    }
}
