using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace Program
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
        public volatile bool Active;
        [JsonIgnore]
        public Global Global;
        [JsonIgnore]
        public List<string> Domains = new List<string>();
        [JsonIgnore]
        public string Path = "www";
        public string[] Extensions { get; set; }
        
        public Modules Modules { get; set; }

        private string _registry = "";
        //private List<Task> activeTasks = new ();
        //public CancellationTokenSource ctsStop = new ();
        
        public Server(){}
        public Server(int port)
        {
            this.Listen = port;
            this.Ip = new IPEndPoint(Dns.GetHostAddresses(Dns.GetHostName())[0], Listen);
            this.Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public Server(string ip, int port)
        {
            this.Listen = port;
            this.Ip = new IPEndPoint(IPAddress.Parse(ip), Listen);
            this.Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Start()
        {
            if (!Active)
            {
                Listener = Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Listener.Bind(Ip);
                Listener.Listen(16);

                Active = true;

                this.Modules.Enabled = this.Global.ModuleEnabled;
                this.Modules.Active = this.Active;
                this.Modules.Start();
                
                GetDomains();
                DomainsRegister();
                this.Global.MySqlServerStart();
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
                this.Modules.Active = this.Active;
                this.Modules.Stop();
                //ctsStop.Cancel();
                Listener.Close();
                DomainsClear();
                //this.Global.SerializeConfig();
                this.Global.MySqlServerClose();
            }  
            else
                Console.WriteLine("Server was stopped");
        }
        public string GetInfo()
        {
            string domain = "";
            if(this.Global.MultipleSite && Domains != null)
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
Active: {Active}
            ";
            return info;
        }
        public string GetStatus()
        {
            return $"Server active: {Active}";
        }
        public void ClientThread(object array)
        {
            Socket client = (Socket)((ArrayList)array)[0];
            Server server= (Server)((ArrayList)array)[1];
            new Client(client, server);
        }
        public void ClientThread(Socket client, Server server)
        {
            new Client(client, server);
        }
        public void GetDomains()
        {
            if(this.Global.MultipleSite)
            {
                foreach (var folder in Directory.GetDirectories($"{AppDomain.CurrentDomain.BaseDirectory}{Path}/"))
                {
                    var dom = folder.Substring(folder.IndexOf("www/")).Replace("www/", "");
                    if (this.Global.Alias.ContainsKey(dom))
                    {
                        Domains.Add(this.Global.Alias[dom]);
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
            string hostsPath = @"C:\Windows\System32\drivers\etc\hosts";
            hosts = File.ReadAllText(hostsPath);
            
            if(this.Global.MultipleSite && Domains != null && Domains.Count != 0 )
            {
                foreach(var domain in Domains)
                {
                    if (this.Global.ListenUse)
                    {
                        hosts += $"\n   {Ip}       {domain}";
                        _registry += $"\n   {Ip}       {domain}";
                    }
                    else
                    {
                        hosts += $"\n   {this.Global.Ipv4}       {domain}";
                        _registry += $"\n   {this.Global.Ipv4}       {domain}";
                    }
                }
            }
            else
            {
                if (this.Global.ListenUse)
                {
                    hosts += $"\n   {Ip}";
                    _registry += $"\n   {Ip}";
                }
                else
                {
                    hosts += $"\n   {this.Global.Ipv4}";
                    _registry += $"\n   {this.Global.Ipv4}";
                }
            }
            File.WriteAllText(hostsPath, hosts);
        }
        public void DomainsClear()
        {
            string hosts = "";
            string hostsPath = @"C:\Windows\System32\drivers\etc\hosts";
            hosts = File.ReadAllText(hostsPath);
            hosts = hosts.Replace(_registry, "");
            File.WriteAllText(hostsPath, hosts);
        }
    }
}
