using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace program
{
    class Global
    {
        public string IPv4 { get; set; }
        public int Listen { get; set; }
        [JsonIgnore]
        public IPAddress Ipv4 {
            get
            {
                if (IPv4 == null || IPv4.Length == 0)
                    IPv4 = "127.0.0.1";
                return IPAddress.Parse(IPv4);
            }
        }
        public Server Server { get; set; }
        public Dictionary<string, string> System { get; set; }

        public Dictionary<string, Interpreter> Interpreters { get; set; }

        // ThreadPool
        public string PoolMin_worker { get; set; }
        public string PoolMin_async { get; set; }
        public string PoolMax_worker { get; set; }
        public string PoolMax_async { get; set; }
        [JsonIgnore]
        public int ThreadPoolMin_worker
        {
            get
            {
                int value = 0;
                if (PoolMin_worker == null || PoolMin_worker.Length == 0)
                    value = 2;
                else if (!int.TryParse(PoolMin_worker, out var val))
                    value = 2;
                else
                    value = val;

                return value;
            }
            set
            {
                PoolMin_worker = value.ToString();
            }
        }
        [JsonIgnore]
        public int ThreadPoolMin_async
        {
            get
            {
                int value = 0;
                if (PoolMin_async == null || PoolMin_async.Length == 0)
                    value = 2;
                else if (!int.TryParse(PoolMin_async, out var val))
                    value = 2;
                else
                    value = val;

                return value;
            }
            set
            {
                PoolMin_async = value.ToString();
            }
        }
        [JsonIgnore]
        public int ThreadPoolMax_worker
        {
            get
            {
                int value = 0;
                if (PoolMax_worker == null || PoolMax_worker.Length == 0)
                    value = 2;
                else if (!int.TryParse(PoolMax_worker, out var val))
                    value = 2;
                else
                    value = val;

                return value;
            }
            set
            {
                PoolMax_worker = value.ToString();
            }
        }
        [JsonIgnore]
        public int ThreadPoolMax_async
        {
            get
            {
                int value = 0;
                if (PoolMax_async == null || PoolMax_async.Length == 0)
                    value = 2;
                else if (!int.TryParse(PoolMax_async, out var val))
                    value = 2;
                else
                    value = val;

                return value;
            }
            set
            {
                PoolMax_async = value.ToString();
            }
        }
        public Global(){}
        public void GetServer()
        {
            Server.global = this;
            Server.Ip = this.Ipv4;
            Server.Listen = this.Listen ;
            GetSystem();
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
            string json = JsonConvert.SerializeObject(global, Formatting.Indented);
            File.WriteAllText(address, json);
        }
        void GetSystem()
        {
            if (System["Bit"] == null || System["Bit"] == "")
            {
                if (Environment.Is64BitOperatingSystem)
                    System["Bit"] = "x64";
                else
                    System["Bit"] = "x86";
            }
            if (System["OS"] == null || System["OS"] == "")
            {
                System["OS"] = Environment.OSVersion.ToString();
            }
            
        }
        
        public string GetInfo()
        {
            string domains = "";
            if (Server.Domains != null || Server.Domains.Count != 0)
            {
                foreach (var i in Server.Domains)
                    domains += $"{i}\n";
            }
            else
                domains = "not init";
            string info = $@"Server C# .Net Core
Ip: {Ipv4}  Post: {Listen}

";
            return info;
        }
    }
}
