using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
//Newtonsoft.Json

namespace program
{
    class Global
    {
        public string IPv4 { get; set; }
        [JsonIgnore]
        public IPAddress Ipv4 { get { return IPAddress.Parse(IPv4); } }
        public string[] Configs { get; set; }
        public List<Server> Servers = new List<Server>();
        public Dictionary<string, string> Includes { get; set; }
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
        public void ServerSerialize()
        {
            if( Configs == null || Configs.Length == 0)
            {
                Console.WriteLine("Configs count = 0; Please add your config in global-config.json");
                return;
            }
            if (!Directory.Exists($"{AppDomain.CurrentDomain.BaseDirectory}/servers"))
            {
                Directory.CreateDirectory($"{AppDomain.CurrentDomain.BaseDirectory}/servers");
            }
            foreach(var elem in Configs)
            {
                bool IsExists = File.Exists(elem);
                if(!IsExists)
                {
                    Console.WriteLine($"Config: {elem} not founded");
                    continue;
                }
                string json = File.ReadAllText(elem);
                json = json.Replace("\\","/");
                Server server = JsonSerializer.Deserialize<Server>(json);
                server.global = this;
                server.Ip = this.Ipv4;
                Servers.Add(server);
                GetSystem();
            }
        }
        public void GlobalSeserialize()
        {
            string address = $@"{AppDomain.CurrentDomain.BaseDirectory}/global-config.json";
            if (!File.Exists(address))
            {
                File.Create(address);
            }
            Global global = this;
            string json = JsonSerializer.Serialize<Global>(global);
            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(global);
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
            int actrive = 0, inactive = 0, servers_count = 0;
            foreach(var i in Servers)
            {
                servers_count++;
                if (i.Active)
                    actrive++;
                else
                    inactive++;
            }
            string info = $@"
Server C# .Net Core
Ip: {Ipv4} 
Servers active: {actrive}   inactive: {inactive}
All server count: {servers_count}
";
            return info;
        }
    }
}
