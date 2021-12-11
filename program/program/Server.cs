using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Diagnostics;

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
        public string Path = "www";
        public string[] Extensions { get; set; }

        string registry = "";
        
        public Server(){ }
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
        ~Server(){
            GC.Collect(2, GCCollectionMode.Forced);
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
                    try
                    {
                        //Console.WriteLine(1);
                        ThreadPool.QueueUserWorkItem(
                            new WaitCallback(ClientThread),
                            new ArrayList() { Listener.Accept(), this }
                            );
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
                DomainsClear();
                Listener.Close();
                Active = false;
                global.SerializeConfig();
                global.MySqlServerClose();
            }  
            else
                Console.WriteLine("Server was stopped");
        }
        public string GetInfo()
        {
            string domain = "";
            if(Domains != null)
            {
                foreach(var elem in Domains)
                {
                    domain += $"{elem} */*";
                }
            }
            else
            {
                domain = "Error outpt domains";
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
            foreach (var folder in Directory.GetDirectories($"{AppDomain.CurrentDomain.BaseDirectory}{Path}/"))
            {
                var dom = folder.Substring(folder.IndexOf("www/")).Replace("www/", "");
                if(global.Alias.ContainsKey(dom))
                {
                    Domains.Add(global.Alias[dom]);
                    continue;
                }
                if (Domains.IndexOf(dom) == -1)
                    Domains.Add(dom);
            }
        }
        public void DomainsRegister()
        {
            string hosts = "";
            string hosts_path = @"C:\Windows\System32\drivers\etc\hosts";
            hosts = File.ReadAllText(hosts_path);
            
            if(Domains != null || Domains.Count != 0)
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
                        hosts += $"\n   {global.IPv4}       {domain}";
                        registry += $"\n   {global.IPv4}       {domain}";
                    }
                }
            }

            File.WriteAllText(hosts_path, hosts);
        }
        public void DomainsClear()
        {
            try
            {
                string hosts = "";
                string hosts_path = @"C:\Windows\System32\drivers\etc\hosts";
                hosts = File.ReadAllText(hosts_path);
                hosts = hosts.Replace(registry, "");
                File.WriteAllText(hosts_path, hosts);
            }
            catch { }
        }
    }
}
