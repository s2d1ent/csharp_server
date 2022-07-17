using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AMES
{
    internal class Configurator
    {
        public string Ipv4 { get; set; }
        public int Port { get; set; }
        public string SslPubKey { get; set; }
        public string SslPrivKey { get; set; }
        public string[] Extensions { get; set; }
        public string Root { get; set; }
        public bool EnabledServerModules { get; set; }
        public bool Multiple { get; set; }
        public bool PhpFastcgi { get; set; }
        public string Index { get; set; }
        public string Php { get; set; }
        public string Python { get; set; }
        public Dictionary<string, string> Alias { get; set; }

        // ThreadPool min&max options
        public int MinWork { get; set; }
        public int MinWorkAsync { get; set; }
        public int MaxWork { get; set; }
        public int MaxWorkAsync { get; set; }

        private AMESLogger _logger = new();
        public Server Server;
        public string _sslPubKey = null;
        public string _sslPrivKey = null;

        public Configurator() { }
        public void Restart()
        {

        }

        public static Configurator Deserialize(string file) 
        {
            Configurator result = new();
            string json = "";    
            if(!File.Exists(file))
            {
                Console.WriteLine("Config not found");
                return null;
            }

            json = File.ReadAllText(file);
            result = JsonConvert.DeserializeObject<Configurator>(json);

            // static data
            SetConstants();
            // Root directory website
            if(result.Root != null && result.Root[0] == '.')
            {
                result.Root = result.Root.Remove(0, 2);
                result.Root = Constants.PATH + result.Root;
            }
            else if(result.Root == "" || result.Root == null)
            {
                result.Root = Constants.PATH_WWW;
            }
            Constants.ROOT = result.Root;
            if(Constants.ROOT[Constants.ROOT.Length - 1] != '/')
            {
                Constants.ROOT += '/';
            }
            // Static extensions
            if(result.Extensions == null || result.Extensions.Length == 0)
            {
                throw new Exception();
            }
            for(int i = 0; i < result.Extensions.Length; i++)
            {
                if(i >= 10)
                {
                    break;
                }
                Constants.EXTENSIONS[i] = result.Extensions[i];
            }
            // Interpreter paths
            Constants.PATH_PHP = result.Php == "" || result.Php == null ? null : result.Php;
            Constants.PATH_PYTHON = result.Python == "" || result.Python == null ? null : result.Python;

            if(Constants.PATH_PHP != null || Constants.PATH_PHP != "")
            {
                
            }
            if(Constants.OS == "Linux")
            {
                Constants.PATH_PHP += Constants.PATH_PHP != null ? "php" : null;
                Constants.PATH_PYTHON += Constants.PATH_PYTHON != null ? "python" : null;
            }
            else if (Constants.OS == "Windows")
            {
                Constants.PATH_PHP += Constants.PATH_PHP != null ? "php.exe" : null;
                Constants.PATH_PYTHON += Constants.PATH_PYTHON != null ? "python.exe" : null;
            }

            Constants.PHPFASTCGI = result.PhpFastcgi;

            result._logger.SetLog(AMESModuleType.Configurator, "Deserialize config");
            return result;
        }
        public void Serialize() 
        {
            FileStream fs = new($@"{AppDomain.CurrentDomain.BaseDirectory}global-config.json", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite, 0, true);
            string json = "";
            json = JsonConvert.SerializeObject(
                this, 
                Formatting.Indented, 
                new JsonSerializerSettings() 
                { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }
            );
        }

        public static void SetConstants()
        {
            Constants.PATH = AppDomain.CurrentDomain.BaseDirectory;
            Constants.PATH_WWW = AppDomain.CurrentDomain.BaseDirectory + "www";            

            if(OperatingSystem.IsLinux())
            {
                Constants.OS = "Linux";
            }
            else if(OperatingSystem.IsWindows())
            {
                Constants.OS = "Windows";
            }
            else if(OperatingSystem.IsAndroid())
            {
                Constants.OS = "Adnroid";
            }
            else if(OperatingSystem.IsMacOS())
            {
                Constants.OS = "MaOS";
            }
            else if(OperatingSystem.IsIOS())
            {
                Constants.OS = "IOS";
            }
            else if(OperatingSystem.IsTvOS())
            {
                Constants.OS = "TvOS";
            }
            else if(OperatingSystem.IsWatchOS())
            {
                Constants.OS = "WatchOS";
            }
            else if(OperatingSystem.IsBrowser())
            {
                Constants.OS = "Browser";
            }
            else if(OperatingSystem.IsFreeBSD())
            {
                Constants.OS = "FreeBSD";
            }
        }

        public Server GetServer()
        {
            if(Ipv4 == null)
            {
                string hostname = System.Net.Dns.GetHostName();
                System.Net.IPAddress[] pool = System.Net.Dns.GetHostAddresses(hostname);
                
                foreach(var elem in pool)
                {
                    if(elem.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        Ipv4 = elem.ToString();
                        break;
                    }
                }
            }
            Server server = new(Ipv4);

            if (Port == 0) 
            {
                Port = 80;
            }

            server.Port = Port;

            server.UsePhp = Php == null ? false : true;
            server.UsePython = Python == null ? false : true;

            server.Multiple = Multiple;
            server.EnabledModules = EnabledServerModules;
            server.Configurator = this;
            
            _logger.SetLog(
                AMESModuleType.Configurator,
                "Configurate server"
            );

            return server;
        }

        public void Init()
        {
            string json = "";
            string path = $@"{AppDomain.CurrentDomain.BaseDirectory}\global-config.json";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0, true);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            fs.Write(buffer, 0, buffer.Length);
        }

        public void CheckedDirectories()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            // doc, includes, modules, logs
            string doc = $@"{path}doc";
            string includes = $@"{path}includes";
            string modules = $@"{path}modules";
            string logs = $@"{path}logs";
            string temp = $@"{path}temp";
            string error = $@"{path}error";

            if (!File.Exists(doc))
            {
                Directory.CreateDirectory(doc);
            }

            if (!File.Exists(includes))
            {
                Directory.CreateDirectory(includes);
            }

            if (!File.Exists(modules))
            {
                Directory.CreateDirectory(modules);
            }

            if (!File.Exists(logs))
            {
                Directory.CreateDirectory(logs);
            }

            if (!File.Exists(temp))
            {
                Directory.CreateDirectory(temp);
            }

            if (!File.Exists(error))
            {
                Directory.CreateDirectory(error);
            }
        }
    }

}
