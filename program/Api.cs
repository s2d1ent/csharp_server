using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private Global _global;

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

        public void Start() { }
        public void Stop() { }
        public void Restart() { }
        public void Help() { }
        public void Aliases() { }
        public void Disconnect() { }
        public void ServerInfo() { }
        public void RerouteResponse() { }
        public void GetConfig() { }
        public void SetConfig() { }
        public void DownloadConfig() { }
        public void LoadConfig() { }
        public void GetConnectInfo() { }
        public void LoadModule() { }
        public void DownLoadModule() { }
        public void UseModule() { }
        public void NoUseModule() { }
        public void ShowModules() { }
        public void ShowIncludes() { }

    }

    public struct ApiData
    {
        public string Rsponse { get; set; }
    }
}
