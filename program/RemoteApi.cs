using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Program
{
    internal class RemoteApi
    {
        public string Ipv4 { get; set; }
        public int Listen = 585;


        private int _listenSecurity = 703;
        private IPEndPoint _ip { get; set; }
        private IPEndPoint _ipSecurity { get; set; }
        private Socket _listener;
        private Socket _listenerSecurity;
        private volatile Global _global;
        private volatile CancellationTokenSource _cts = new();

        private string[,] Users { get; set; }

        public RemoteApi(Global global)
        {
            this.Ipv4 = global.Ipv4;
            this._ip = new IPEndPoint(
                IPAddress.Parse(this.Ipv4), this.Listen
                );
            this._global = global;

            this._listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Start() 
        {
            _listener.Bind(_ip);
            _listener.Listen(8);
            
            while(!_cts.IsCancellationRequested)
            {
                Task.Run(
                        ()=> { RemoteApi.UserConnect(_listener.Accept()); }
                       , _cts.Token
                    );
            }
        }
        public void Stop()
        {
            _cts.Cancel();
            _listener.Close();
            
        }
        public void Restart() 
        {
            Stop();
            _cts = new();
            Start();
        }
        public void Help() { }
        public void Aliases() { }
        public void Disconnect() { }
        public void ServerInfo() { }
        public void RerouteResponse() 
        {
            
        }
        // config
        public void GetConfig() 
        {
            
        }
        public void SetConfig() { }
        public byte[] DownloadConfig(string file) 
        {
            string app = AppDomain.CurrentDomain.BaseDirectory;
            string path = app + @"\global-config.json";
            FileStream fs = new(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096 ,true);
            byte[] buffer = new byte[fs.Length];
            fs.ReadAsync(buffer, 0, buffer.Length, _cts.Token);
            return buffer;
        }
        public void LoadConfig() { }
        public void GetConnectInfo() { }
        // modules
        public void LoadModule() { }
        public void DownLoadModule() { }
        public void UseModule() { }
        public void NoUseModule() 
        {
            
        }
        public string ShowModules() 
        {
            string response = "";
            string app = AppDomain.CurrentDomain.BaseDirectory;
            string includes = app + @"\modules\";

            foreach (var files in Directory.GetFiles(includes))
            {
                if(files.EndsWith(".dll"))
                {
                    response += files + "\n";
                }
            }

            return response;
        }
        // includes
        public string ShowIncludes() 
        {
            string response = "";
            string app = AppDomain.CurrentDomain.BaseDirectory;
            string includes = app + @"\includes\";

            foreach(var folder in Directory.GetDirectories(includes))
            {
                response += folder + "\n";
            }

            return response;
        }

        public static void UserConnect(Socket user)
        {
            if (user == null) return;
        }

        public static void UserSecureConnect(Socket user)
        {
            if (user == null) return;
        }
    }

    public struct ApiData
    {
        public string Rsponse { get; set; }
        public string Request { get; set; }
        public bool isStandartCmd;
    }
}
