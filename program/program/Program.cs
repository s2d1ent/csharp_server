using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace program
{
    class Program
    {
        /**/
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Global global = SerialaizeGlobal($@"{path}/global-config.json");
            // min&max thread will be used
            ThreadPool.SetMinThreads(global.ThreadPoolMin_worker, global.ThreadPoolMin_async);
            ThreadPool.SetMinThreads(global.ThreadPoolMax_worker, global.ThreadPoolMax_async);
            // get server
            global.GetServer();
            Console.WriteLine(global.GetInfo());
            while (true)
                {
                string cmd = Console.ReadLine();
                if(cmd == "clear")
                {
                    Console.Clear();
                }
                if(cmd == "start")
                {
                    global.Server.StartAsync();
                    global.MySqlServerStartAsync();
                }
                if (cmd.IndexOf("restart")!=-1)
                {
                    if(cmd == "restart")
                    {
                        Console.Clear();
                        global.MySqlServerClose();
                        global.Server.Stop();
                        global = SerialaizeGlobal($@"{path}/global-config.json");
                        ThreadPool.SetMinThreads(global.ThreadPoolMin_worker, global.ThreadPoolMin_async);
                        ThreadPool.SetMinThreads(global.ThreadPoolMax_worker, global.ThreadPoolMax_async);
                        global.GetServer();
                        Console.WriteLine(global.GetInfo());
                        global.Server.StartAsync();
                        global.MySqlServerStartAsync();
                    }
                }
                if (cmd == "stop")
                {
                    global.Server.Stop();
                    global.MySqlServerCloseAsync();
                }
                if(cmd == "exit")
                {
                    global.MySqlServerClose();
                    global.Server.Stop();
                    global.SerializeConfig();
                    return;
                }
                if(cmd.IndexOf("server") != -1)
                {
                    if(cmd == "server")
                    {
                        Console.WriteLine(global.GetInfo());
                    }
                    if(cmd.IndexOf("--status") != -1)
                    {
                        Console.WriteLine(global.Server.GetStatus());
                    }
                }
                if (cmd.IndexOf("help") != -1)
                {

                }
            }
        }
        static Global SerialaizeGlobal(string address)
        {
            Global result = new Global();
            try
            {
                string json = File.ReadAllText(address);
                json = json.Replace("\\", "/");
                result = JsonSerializer.Deserialize<Global>(json);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return result;
        }
    }
}
