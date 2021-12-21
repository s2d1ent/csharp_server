using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace program
{
    class Server
    {
        [JsonIgnore]
        public EndPoint Ip;
        [JsonIgnore]
        public int Listen { get; set; }
        [JsonIgnore]
        Socket Listener;
        [JsonIgnore]
        public bool Active;
        [JsonIgnore]
        public Global global;
        [JsonIgnore]
        public List<string> Domains = new List<string>();
        [JsonIgnore]
        public string Path = "www";
        public string[] Extensions { get; set; }

        private string registry = "";
        //private List<Task> activeTasks = new ();
        //public CancellationTokenSource ctsStop = new ();
        
        public Server(){}
        public Server(int port)
        {
            this.Listen = port;
            this.Ip = new IPEndPoint(Dns.GetHostAddresses(Dns.GetHostName())[0], Listen);
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public Server(string ip, int port)
        {
            this.Listen = port;
            this.Ip = new IPEndPoint(IPAddress.Parse(ip), Listen);
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Start()
        {
            if (!Active)
            {
                Listener = Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Listener.Bind(Ip);
                Listener.Listen(16);

                Active = true;
                GetDomains();
                DomainsRegister();
                global.MySqlServerStart();
                Console.WriteLine(GetInfo());
                while (Active)
                {
                    // TODO - ctsToken
                    try
                    {
                        ThreadPool.QueueUserWorkItem(
                            new WaitCallback(ClientThread),
                            new ArrayList() { Listener.Accept(), this }
                        );
                    } catch { }
                }
            }
            else
                Console.WriteLine("Server was started");
        }
        public async void StartAsync()
        {
            await Task.Run(()=> { Start(); });
        }
        // TODO - stop with ctsToken
        public void Stop()
        {
            if (Active)
            {
                Active = false;
                //ctsStop.Cancel();
                Listener.Close();
                DomainsClear();
                global.SerializeConfig();
                global.MySqlServerClose();
            }  
            else
                Console.WriteLine("Server was stopped");
        }
        public string GetInfo()
        {
            string domain = "";
            if(global.MultipleSite && Domains != null)
            {
                foreach(var elem in Domains)
                {
                    domain += $"{elem} */*";
                }
            }
            else
            {
                domain = "Multiplesite mode is False";
            }
            string info = @$"Domain: {domain}
Ip: {Ip}    Port: {Listen}
Active: {Active}
            ";
            return info;
        }
        public string GetStatus()
        {
            return $"Server active: {Active}";
        }
        public void ClientThread(object client)
        {
            Socket c = (Socket)((ArrayList)client)[0];
            Server s = (Server)((ArrayList)client)[1];
            new Client(c, s);
        }
        public void ClientThread(Socket client, Server server)
        {
            new Client(client, server);
        }
        public void GetDomains()
        {
            if(global.MultipleSite)
            {
                foreach (var folder in Directory.GetDirectories($"{AppDomain.CurrentDomain.BaseDirectory}{Path}/"))
                {
                    var dom = folder.Substring(folder.IndexOf("www/")).Replace("www/", "");
                    if (global.Alias.ContainsKey(dom))
                    {
                        Domains.Add(global.Alias[dom]);
                        continue;
                    }
                    if (Domains.IndexOf(dom) == -1)
                        Domains.Add(dom);
                }
            }
        }
        public void DomainsRegister()
        {
            string hosts = "";
            string hosts_path = @"C:\Windows\System32\drivers\etc\hosts";
            hosts = File.ReadAllText(hosts_path);
            
            if(global.MultipleSite && Domains != null && Domains.Count != 0 )
            {
                foreach(var domain in Domains)
                {
                    if (global.ListenUse)
                    {
                        hosts += $"\n   {Ip}       {domain}";
                        registry += $"\n   {Ip}       {domain}";
                    }
                    else
                    {
                        hosts += $"\n   {global.Ipv4}       {domain}";
                        registry += $"\n   {global.Ipv4}       {domain}";
                    }
                }
            }
            else
            {
                if (global.ListenUse)
                {
                    hosts += $"\n   {Ip}";
                    registry += $"\n   {Ip}";
                }
                else
                {
                    hosts += $"\n   {global.Ipv4}";
                    registry += $"\n   {global.Ipv4}";
                }
            }
            File.WriteAllText(hosts_path, hosts);
        }
        public void DomainsClear()
        {
            string hosts = "";
            string hosts_path = @"C:\Windows\System32\drivers\etc\hosts";
            hosts = File.ReadAllText(hosts_path);
            hosts = hosts.Replace(registry, "");
            File.WriteAllText(hosts_path, hosts);
        }
    }
}
