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

        private ApiUser _user = new();
        private ApiUser[] _users { get; set; }
        private ApiAccess _access = ApiAccess.None;
        

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
        public byte[] Help() 
        {
            string result = "";
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\help";
            if (File.Exists(path))
            {
                result = File.ReadAllText(path);
            }
            else
            {
                result = "basedirectory/help file not found. Plase try soon again.";
            }
            byte[] buffer = Encoding.UTF8.GetBytes(result);
            return buffer;
        }
        public byte[] Aliases() 
        {
            string result = "";

            foreach(var alias in _global.Alias)
            {
                result += $"Folder: {alias.Value} Alias: {alias.Key}\n";
            }

            byte[] buffer = Encoding.UTF8.GetBytes(result);
            return buffer;
        }
        public void Disconnect() { }
        public byte[] ServerInfo() 
        {
            string result = "";

            result = $"C# .Net Core Server\nIp: {_global.Ipv4}:{_global.Listen}\n";

            byte[] buffer = Encoding.UTF8.GetBytes(result);
            return buffer;
        }
        public byte[] RerouteResponse(Response response) 
        {
            byte[] buffer = Encoding.UTF8.GetBytes(response.Data);
            return buffer;
        }
        // config
        public byte[] GetConfig() 
        {
            string result = "";
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if (File.Exists($@"{path}/global-config.json"))
            {
                result = $@"{path}/global-config.json";
            }
            else
            {
                result = $"{path}global-config.json not found use: program -config init";
                
            }
            byte[] buffer = Encoding.UTF8.GetBytes(result);
            return buffer;
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
        public void LoadConfig() 
        { 
        
        }
        public void GetConnectInfo() 
        {
        
        }
        // modules
        public void LoadModule() { }
        public byte[] DownLoadModule(string file) 
        {
            FileStream fs = new(file, FileMode.Open, FileAccess.Read , FileShare.ReadWrite, 0, true);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);

            return buffer;
        }
        public void UseModule() 
        { 
            
        }
        public void NoUseModule() 
        {
            
        }
        public byte[] ShowModules() 
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

            byte[] buffer = Encoding.UTF8.GetBytes(response);
            return buffer;
        }
        // includes
        public byte[] ShowIncludes() 
        {
            string response = "";
            string app = AppDomain.CurrentDomain.BaseDirectory;
            string includes = app + @"\includes\";

            foreach(var folder in Directory.GetDirectories(includes))
            {
                response += folder + "\n";
            }
            byte[] buffer = Encoding.UTF8.GetBytes(response);
            return buffer;
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

    internal enum ApiAccess
    {
        None,
        Public,
        Private
    }

    internal struct ApiUser
    {
        public string Name;
        public string Password;

        public void Set(string name, string password)
        {
            this.Name = name;
            this.Password = password;
        }

        public static ApiUser User(string name, string password)
        {
            ApiUser result = new();
            result.Set(name, password);
            return result;
        }

    }

    internal struct ApiData
    {
        public string Rsponse { get; set; }
        public string Request { get; set; }
        private bool _isStandartCmd;
        private bool _isModuleCmd;
        public string[] ModulesCmd { get; set; }
    }
}
