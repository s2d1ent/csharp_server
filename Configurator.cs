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
        public ServerMode ServerMode { get; set; }
        public string SslPubKey { get; set; }
        public string SslPrivKey { get; set; }
        public string[] Extensions { get; set; }
        public string Root { get; set; }
        public bool EnabledServerModules { get; set; }
        public bool PhpFastcgi { get; set; }
        public string Php { get; set; }
        public string Python { get; set; }
        public Dictionary<string, string> Alias { get; set; }
        public Cache Cache;

        // ThreadPool min&max options
        public int MinWork { get; set; }
        public int MinWorkAsync { get; set; }
        public int MaxWork { get; set; }
        public int MaxWorkAsync { get; set; }

        private AMESLogger _logger = new AMESLogger();
        public Server Server;
        public List<Server> Containers { get; set; }
        public RemoteApi RemoteApi { get; set;}
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

            // [STATIC DATA BEGIN]
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
            if(Constants.OS == OperationsSystem.Linux)
            {
                Constants.PATH_PHP += Constants.PATH_PHP != null ? "php" : null;
                Constants.PATH_PYTHON += Constants.PATH_PYTHON != null ? "python" : null;
            }
            else if (Constants.OS == OperationsSystem.Windows)
            {
                Constants.PATH_PHP += Constants.PATH_PHP != null ? "php.exe" : null;
                Constants.PATH_PYTHON += Constants.PATH_PYTHON != null ? "python.exe" : null;
            }

            Constants.PHPFASTCGI = result.PhpFastcgi;
            // [STATIC DATA END]
            result.GetRApi();  
            
            result.GetIP(ref result);
            result.Cache = new Cache();

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

        public void GetIP(ref Configurator configurator)
        {
            if(configurator.Ipv4 == null)
            {
                string hostname = System.Net.Dns.GetHostName();
                System.Net.IPAddress[] pool = System.Net.Dns.GetHostAddresses(hostname);
                
                foreach(var elem in pool)
                {
                    if(elem.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        configurator.Ipv4 = elem.ToString();
                        break;
                    }
                }
            }
        }

        public static void SetConstants()
        {
            Constants.PATH = AppDomain.CurrentDomain.BaseDirectory;
            Constants.PATH_WWW = AppDomain.CurrentDomain.BaseDirectory + "www";            

            if(OperatingSystem.IsLinux())
            {
                Constants.OS = OperationsSystem.Linux;
            }
            else if(OperatingSystem.IsWindows())
            {
                Constants.OS =  OperationsSystem.Windows;
            }
            else if(OperatingSystem.IsAndroid())
            {
                Constants.OS = OperationsSystem.Android;
            }
            else if(OperatingSystem.IsMacOS())
            {
                Constants.OS = OperationsSystem.MacOS;
            }
            else if(OperatingSystem.IsIOS())
            {
                Constants.OS = OperationsSystem.IOS;
            }
            else if(OperatingSystem.IsTvOS())
            {
                Constants.OS = OperationsSystem.TvOs;
            }
            else if(OperatingSystem.IsWatchOS())
            {
                Constants.OS = OperationsSystem.watchOS;
            }
            else if(OperatingSystem.IsBrowser())
            {
                Constants.OS = OperationsSystem.Browser;
            }
            else if(OperatingSystem.IsFreeBSD())
            {
                Constants.OS = OperationsSystem.FreeBSD;
            }
            else
            {
                Constants.OS = OperationsSystem.NONE;
            }
        }

        public void InitContainers()
        {

        }

        public Server GetServer()
        {
            if (Port == 0) 
            {
                Port = 80;
            }
            Server server = new(Ipv4, Port);

            server.UsePhp = Php == null ? false : true;
            server.UsePython = Python == null ? false : true;

            server.EnabledModules = EnabledServerModules;
            server.Configurator = this;
            
            _logger.SetLog(
                AMESModuleType.Configurator,
                "Configurate server"
            );

            return server;
        }

        public void GetRApi()
        {
            if(RemoteApi == null)
            {
                RemoteApi = new RemoteApi(Ipv4);
            }
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
            string hosts = $@"{path}hosts.txt";

            if (!Directory.Exists(doc))
            {
                Directory.CreateDirectory(doc);
            }

            if (!Directory.Exists(includes))
            {
                Directory.CreateDirectory(includes);
            }

            if (!Directory.Exists(modules))
            {
                Directory.CreateDirectory(modules);
            }

            if (!Directory.Exists(logs))
            {
                Directory.CreateDirectory(logs);
            }

            if (!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);
            }

            if (!Directory.Exists(error))
            {
                Directory.CreateDirectory(error);
            }

            if (!File.Exists(hosts))
            {
                File.Create(hosts);
            }
        }
    }

    internal enum ServerMode
    {
        // NONE
        NONE = 0,
        // when in dir './www' can be only one site; './www' is root dir
        Single,
        // when in dir './www' can be multiple dir, when each dir is site
        Multiple,
        // when in dir './www' can be multiple dir and under dir configurator create self listener
        Container
    }

    internal enum OperationsSystem
    {
        NONE,
        Linux,
        Windows,
        FreeBSD,
        Android,
        MacOS,
        IOS,
        TvOs,
        watchOS,
        Browser
    }
}
