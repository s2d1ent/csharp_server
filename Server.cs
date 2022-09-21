//     AMES(Application Modular Extensible Server) This is a simple web server which is a tutorial
//     Copyright (C) 2022 Viktor Tyumenev
//
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
//
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <https://www.gnu.org/licenses/>.
//
//      Email: tumenev33@mail.ru
//      Email: vornfrost@gmail.com

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace AMES
{
    internal class Server
    {
        [JsonIgnore]
        public string Ipv4 { get; set; }
        [JsonIgnore]
        public int Port { get; set; }
        [JsonIgnore]
        public string SslPubKey { get; set; }
        [JsonIgnore]
        public string SslPrivKey { get; set; }
        [JsonIgnore]
        public bool Active;
        [JsonIgnore]
        public bool EnabledModules;
        [JsonIgnore]
        public Configurator Configurator {get;set;}
        [JsonIgnore]
        public CancellationTokenSource Cts
        {
            get
            {
                return _cts;
            }
            set
            {
                if (_cts == null || _cts.IsCancellationRequested) _cts = value;
            }
        }
        [JsonIgnore]
        public Modules Modules
        {
            get 
            {
                return _modules;
            }
            set 
            {
                _modules = value;
            }
        }
        [JsonIgnore]
        private Modules _modules;
        private IPEndPoint _ip;
        private Socket _listener;
        private volatile CancellationTokenSource _cts;
        private List<string> Domains;
        private DateTime _startServer;

        private AMESLogger _logger;

        // extensions
        [JsonIgnore]
        public bool UsePhp { get; set; }
        [JsonIgnore]
        public bool UsePython { get; set; }

        public Server()
        {
            this.Ipv4 = "127.0.0.1";
            this.Port = 80;
            this._ip = new IPEndPoint(
                IPAddress.Parse(this.Ipv4), this.Port
                );
            this._modules = new();
            this._cts = new();
            this._listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._logger = new();
            this._startServer = DateTime.Now;
        }
        public Server(string ip)
        {
            this.Ipv4 = ip;
            this.Port = 80;
            this._ip = new IPEndPoint(
                IPAddress.Parse(this.Ipv4), this.Port
                );
            this._modules = new();
            this._cts = new();
            this._listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._logger = new();
            this._startServer = DateTime.Now;
        }

         public Server(string ip, int port)
        {
            this.Ipv4 = ip;
            this.Port = port;
            this._ip = new IPEndPoint(
                IPAddress.Parse(this.Ipv4), this.Port
                );
            this._modules = new();
            this._cts = new();
            this._listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._logger = new();
            this._startServer = DateTime.Now;
        }

        public void Start() 
        {
            if(!Active)
            {
                Active = true;
                _listener.Bind(_ip);
                _listener.Listen();
                if(Configurator.ServerMode == ServerMode.Multiple)
                {
                    InitMultiple();
                }
                Console.WriteLine(GetInfo());

                _logger.SetLog(
                    AMESModuleType.Server,
                    $"Start the server {this.Ipv4}:{this.Port}"
                );

                while (Active || !_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        Socket listenerAccept = _listener.Accept();
                        if (listenerAccept != null)
                        {
                            Task.Run(
                                () => Client(listenerAccept, Configurator),
                                _cts.Token
                            );
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
        public void Stop() 
        {
            if(Active)
            {
                _cts.Cancel();
                Active = false;
                _listener.Close();
                _modules.Stop();
                if(Configurator.ServerMode == ServerMode.Multiple)
                {
                    UninitMultiple();
                }
                _logger.SetLog(
                    AMESModuleType.Server,
                    $"Stop the server {this.Ipv4}:{this.Port}"
                );
            }
        }
        public async void StartAsync() 
        {
            await Task.Run(() => Start());
        }
        public async void StopAsync() 
        {
            await Task.Run(() => Stop());
        }
        public string GetInfo()
        {
            string result = "AMES  Copyright (C) 2022  Viktor Tyumenev\n";
            result += "This program comes with ABSOLUTELY NO WARRANTY; for details type `--license'.\n";
            result += "This is free software, and you are welcome to redistribute it\n";
            result += "under certain conditions; type `--license' for details.\n\n";
            result += $"{Constants.FULLNAME}({Constants.NAME})\n";
            result += $"{Constants.NAME} v{Constants.VERSION}\n";
            result += $"License {Constants.LICENSE}\n";
            result += $"Dev files: {Constants.DISTRIBUTIVE} \n";
            result += $"Addres: {this.Ipv4} Port: {this.Port}\n";
            return result;
        }
        private void Client(Socket socket, Configurator configurator)
        {
            Client client = new Client(socket, configurator);
            client.Start();
        }

        private void InitMultiple()
        {
            string hosts = "";
            string appendText = "";
            string[] directories = Directory.GetDirectories("./www/");
            
            hosts = (Constants.OS == OperationsSystem.Linux || Constants.OS == OperationsSystem.MacOS 
            || Constants.OS == OperationsSystem.FreeBSD || Constants.OS == OperationsSystem.NONE
            || Constants.OS == OperationsSystem.Android || Constants.OS == OperationsSystem.IOS) ? "/etc/hosts" :
            (
                (Constants.OS == OperationsSystem.Windows) ? "C:\\Windows\\System32\\drivers\\etc\\hosts" : "/etc/hosts"
            );

            // if(File.Exists("./hosts.txt"))
            // {
            //     appendText = File.ReadAllText("./hosts.txt");
            //     if(appendText.Length != 0)
            //     {
            //         File.WriteAllText(hosts, 
            //             File.ReadAllText(hosts).Replace(appendText, "")
            //         );
            //         appendText = "";
            //     }
            // }

            // foreach(string val in directories)
            // {
            //     appendText += Ipv4 + " " + new DirectoryInfo(val).Name + '\n';
            // }

            // File.AppendAllText(hosts, appendText);
            // using(File.Create("./hosts.txt"));
            // File.AppendAllText("./hosts.txt", appendText);
        }

        private void UninitMultiple()
        {
            string hosts = "";
            string hostsText = "";
            string appendText = "";
            
            hosts = (Constants.OS == OperationsSystem.Linux || Constants.OS == OperationsSystem.MacOS 
            || Constants.OS == OperationsSystem.FreeBSD || Constants.OS == OperationsSystem.NONE
            || Constants.OS == OperationsSystem.Android || Constants.OS == OperationsSystem.IOS) ? "/etc/hosts" :
            (
                (Constants.OS == OperationsSystem.Windows) ? "C:\\Windows\\System32\\drivers\\etc\\hosts" : "/etc/hosts"
            );
            appendText = File.ReadAllText("./hosts.txt");
            hostsText = File.ReadAllText(hosts);
            hostsText = hostsText.Replace(appendText, "");

            File.AppendAllText(hosts, appendText);
        }
    }
}
