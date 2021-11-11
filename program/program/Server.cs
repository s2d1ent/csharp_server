using System;
using System.Collections;
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
        public IPAddress Ip;
        TcpListener Listener;
        public int Listen { get; set; }
        public bool Active;
        public Global global;
        public string Domain { get; set; }
        public string Path { get; set; }
        public string[] Extensions { get; set; }

        public string Framework_py { get; set; }
        
        public Server(){ }
        public Server(int port)
        {
            this.Listen = port;
            this.Ip = Dns.GetHostAddresses(Dns.GetHostName())[0];
            Listener = new TcpListener(this.Ip, port);
        }
        public Server(string ip, int port)
        {
            this.Listen = port;
            this.Ip = IPAddress.Parse(ip);
            Listener = new TcpListener(IPAddress.Parse(ip), port);
        }
        public void Start()
        {
            if (!Active)
            {
                if (Listener == null)
                    Listener = new TcpListener(Ip, Listen);
                Listener.Start();
                Active = true;
                Console.WriteLine(GetInfo());
                while (true)
                {
                    try
                    {
                        //Console.WriteLine(1);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), new ArrayList() { Listener.AcceptTcpClient(), this });
                    }
                    catch (Exception ex) { }
                }
            }
            else
                Console.WriteLine("Server was started");
        }
        public async void StartAsync()
        {
            await Task.Run(()=> { Start(); });
        }
        public void Stop()
        {
            if (Active)
            {
                Listener.Stop();
                Active = false;
                global.GlobalSeserialize();
            }  
            else
                Console.WriteLine("Server was started");
        }
        public string GetInfo()
        {
            string info = @$"Domain: {Domain}
Ip: {Ip}    Port: {Listen}
Active: {Active}
            ";
            return info;
        }
        public string GetStatus()
        {
            return $"Server active: {Active}";
        }
        public void OpenDocumentation()
        {

        }
        public void ClientThread(object client)
        {
            TcpClient c = (TcpClient)((ArrayList)client)[0];
            Server s = (Server)((ArrayList)client)[1];
            new Client(c, s);
        }
        public void JsonConfig()
        {

        }
    }
}
