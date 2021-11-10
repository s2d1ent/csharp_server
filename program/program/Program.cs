using System;

namespace program
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(80);
            server.Start();
            while(true)
            {
                string cmd = Console.ReadLine();
                if(cmd == "start")
                {
                    server.Start();
                }
                if(cmd == "stop")
                {
                    server.Stop();
                }
                if(cmd == "exit")
                {
                    server.Stop();
                    return;
                }
                if(cmd.IndexOf("server") != -1)
                {
                    if(cmd == "server")
                    {
                        Console.WriteLine(server.GetInfo());
                    }
                    if(cmd.IndexOf("--status") != -1)
                    {
                        Console.WriteLine(server.GetStatus());
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
