using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            string link = @"C:\Users\Admin\Desktop\csharp_server\program\program\bin\Debug\net5.0\includes\php\win86\php-cgi.exe",
                text = @"C:\Users\Admin\Desktop\csharp_server\program\program\bin\Debug\net5.0\www\UPPDD\php\auth.php";
            Console.WriteLine(UseInterpreter(link, text));
            Console.ReadKey();
        }
        static string UseInterpreter(string php, string file)
        {
            ProcessStartInfo info = new ProcessStartInfo(php);//, file
         //   var len = File.ReadAllBytes(file).Length;
           // Console.WriteLine(len);
            info.UseShellExecute = false;
            info.ErrorDialog = false;
            info.RedirectStandardError = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.CreateNoWindow = true;
            info.EnvironmentVariables.Add("SCRIPT_FILENAME", file);
          //  info.EnvironmentVariables.Add("CONTENT_LENGTH", $"{len}");
            //info.EnvironmentVariables.Add("REDIRECT_STATUS", "true");
            info.EnvironmentVariables.Add("REQUEST_METHOD", "POST");
            info.EnvironmentVariables.Add("CONTENT_TYPE", "application/x-www-form-urlencoded");
            info.EnvironmentVariables.Add("QUERY_STRING", "login=admin&pass=secretpass&but=%D0%92%D0%BE%D0%B9%D1%82%D0%B8");
            //info.EnvironmentVariables.Add("SERVER_NAME", $@"http://uppdd/");
            //info.EnvironmentVariables.Add("SERVER_PORT", $@"80");
            Process p = new Process();
            p.StartInfo = info;

            bool pStarted = p.Start();

            StreamWriter input = p.StandardInput;
            StreamReader output = p.StandardOutput;
            StreamReader error = p.StandardError;

            byte[] cp866_byte = output.CurrentEncoding.GetBytes(output.ReadToEnd());
            string cp866_byte_to_utf8 = Encoding.UTF8.GetString(cp866_byte);

            return cp866_byte_to_utf8+ "\n"+error.ReadToEnd();
        }
    }
}
