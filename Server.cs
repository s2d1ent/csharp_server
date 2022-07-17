using System;
using System.Collections.Generic;
using System.Linq;
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
        public bool Multiple { get; set; }
        public string Index { get; set; }
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
        }

        public void Start() 
        {
            if(!Active)
            {
                Active = true;
                _listener.Bind(_ip);
                _listener.Listen();

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
            
            string result = $"{Constants.FULLNAME}({Constants.NAME})\n";
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
    }
}
