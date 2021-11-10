using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace program
{
    class Server
    {
        IPAddress Ip;
        TcpListener Listener;
        int Port;
        public bool Active;
        public Server(int port)
        {
            this.Port = port;
            this.Ip = Dns.GetHostAddresses(Dns.GetHostName())[0];
            Listener = new TcpListener(this.Ip, port);
        }
        public Server(string ip, int port)
        {
            this.Port = port;
            this.Ip = IPAddress.Parse(ip);
            Listener = new TcpListener(IPAddress.Parse(ip), port);
        }
        public void Start()
        {
            Listener.Start();
            Console.WriteLine(GetInfo());
            Task.Run(()=> {
                while(true)
                {
                    try
                    {
                        ThreadPool.SetMinThreads(2, 2);
                        ThreadPool.SetMinThreads(4, 4);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), Listener.AcceptTcpClient());
                    }
                    catch (Exception ex) { }
                }
            });
        }
        public void Stop()
        {
            Listener.Stop();
        }
        public string GetInfo()
        {
            string info = @$"Tesr server C# .Net
Host name: {Dns.GetHostName()}
Ip: {Ip}    Port: {Port}
            ";
            return info;
        }
        public string GetStatus()
        {
            return $"Server active: {Active}";
        }
        public void ClientThread(object client)
        {
            new Client((TcpClient)client);
        }
    }
}
