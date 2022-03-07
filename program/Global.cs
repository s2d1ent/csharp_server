using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Program
{
    class Global
    {
        public string Ipv4 { get; set; }
        public int Listen { get; set; }
        public bool ListenUse { get; set; }
        public bool MultipleSite { get; set; }
        public bool ModuleEnabled { get; set; }
        public Modules Modules { get; set; }
        public Server Server { get; set; }
        public Dictionary<string, string> Alias { get; set; }

        public Dictionary<string, Interpreter> Interpreters { get; set; }
        public string[] MySqlPath { get; set; }
        
        [JsonIgnore]
        public RemoteApi RemoteApi
        {
            get
            {
                return null;
            }
            set
            {
                if(_remoteApi == null)
                {
                    _remoteApi = value;
                }
            }
        }

        private volatile RemoteApi _remoteApi;

        // ThreadPool
        public int MinWorker { get; set; }
        public int MinWorkerAsync { get; set; }
        public int MaxWorker { get; set; }
        public int MaxWorkerAsync { get; set; }

        // constructors
        public Global(){}

        // methods
        public void GetServer()
        {
            Server.Global = this;
            if (Listen != 0) Server.Listen = this.Listen;
            else if (Listen == 0 || Listen < 0) Server.Listen = 80;
            if (Ipv4 != "") Server.Ip = new IPEndPoint(IPAddress.Parse(Ipv4.ToString()), Listen);
            else
            {
                IPEndPoint IP = new IPEndPoint(Dns.GetHostAddresses(Dns.GetHostName())[0], Listen);
                this.Ipv4 = Dns.GetHostAddresses(Dns.GetHostName())[0].ToString();
                Server.Ip = IP;
            }
            
        }


        public void SerializeConfig()
        {
            string address = $@"{AppDomain.CurrentDomain.BaseDirectory}/global-config.json";
            if (!File.Exists(address))
            {
                File.Create(address);
            }
            Global global = this;
            //string json = JsonSerializer.Serialize<Global>(global);
            string json = JsonConvert.SerializeObject(global, 
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            File.WriteAllText(address, json);
        }

        public void MySqlServerStart()
        {
            ProcessStartInfo info = new ProcessStartInfo($"{AppDomain.CurrentDomain.BaseDirectory}{MySqlPath[0]}");
            info.UseShellExecute = false;
            info.ErrorDialog = false;
            info.RedirectStandardError = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.CreateNoWindow = true;

            Process p = new Process();
            p.StartInfo = info;

            bool pStarted = p.Start();
        }
        public void MySqlServerClose()
        {
            ProcessStartInfo info = new ProcessStartInfo($"{AppDomain.CurrentDomain.BaseDirectory}{MySqlPath[1]}", "-u root shutdown");
            info.UseShellExecute = false;
            info.ErrorDialog = false;
            info.RedirectStandardError = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.CreateNoWindow = true;
            
            Process p = new Process();
            p.StartInfo = info;

            bool pStarted = p.Start();
        }


        public async void MySqlServerStartAsync()
        {
            await Task.Run(()=> { MySqlServerStart(); });
        }
        public async void MySqlServerCloseAsync()
        {
            await Task.Run(() => { MySqlServerClose(); });
        }


        public string GetInfo()
        {
            string domains = "";
            if (MultipleSite && Server.Domains != null || Server.Domains.Count != 0)
            {
                foreach (var i in Server.Domains)
                    domains += $"{i}\n";
            }
            else
                domains = "not found";
            string info = $@"Server C# .Net Core
Ip: {Ipv4}  Port: {Listen}
";
            return info;
        }
    }
}
