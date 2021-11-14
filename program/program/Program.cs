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
            // create 
            global.GetServer();
            Console.WriteLine(global.GetInfo());
            //Server server = new Server(80);
            //server.Start();
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
                }
                if(cmd == "stop")
                {
                    global.Server.Stop();
                }
                if(cmd == "exit")
                {
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
                        
                        /*for (var i = 0; i < global.Servers.Count; i++)
                        {
                            //Console.WriteLine($"{i}: {global.Servers[i].Domain} active: {global.Servers[i].Active}");
                        }*/
                    }
                    if (cmd.IndexOf("show")!=-1)
                    {
                        /*for(var i = 0; i < global.Servers.Count; i++)
                        {
                            //Console.WriteLine($"{i}: {global.Servers[i].Domain}\nIp: {global.Servers[i].Ip} Port: {global.Servers[i].Listen}/nPath: {global.Servers[i].Path}");
                        }*/
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
