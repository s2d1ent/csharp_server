using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Loader;
using Newtonsoft.Json;

namespace Program
{
    public class Modules : AssemblyLoadContext
    {
        //public property/field
        public string[] Dlls { get; set; }
        [JsonIgnore]
        public bool Active { get; set; }
        //private property/field
        [JsonIgnore]
        private List<Module> _dlls = new ();

        // public methods
        public Modules() : base(isCollectible:true) { }
        public void Start()
        {
            this.Init();
            this.StartAsync();
        }
        public void Stop()
        {
            this.Unload();
        }
        // private methods
        private async void StartAsync()
        {
            await Task.Run(()=> {
                while(Active)
                {
                    foreach (var module in _dlls)
                    {
                        module.Activate();
                    }
                }
            });
        }
        private void Init()
        {
            foreach(var obj in Dlls)
            {
                if (File.Exists(obj))
                {
                    Assembly externModule = this.LoadFromAssemblyPath(obj);
                    Type classModule = externModule.GetType("Module");
                    Module module = new();
                    module.Path = obj;
                    module.MethodInfo = classModule.GetMethod("Extern");
                    _dlls.Add(module);
                }
                else
                {
                    Console.WriteLine($"Module not found\nPath: {obj}");
                }
            }
            
        }

    }
    public class Module
    {
        //public propertys/fields
        [JsonIgnore]
        public string Path {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }
        [JsonIgnore]
        public MethodInfo MethodInfo {
            get
            {
                return _methodInfo;
            }
            set
            {
                _methodInfo = value;
            }
        }
        //private [rp[ertys/fields
        [JsonIgnore]
        private string _path;
        [JsonIgnore]
        private MethodInfo _methodInfo;

        public void Activate()
        {
            object moduleInstance = Activator.CreateInstance(_methodInfo.ReflectedType);
            object[] parameters = new object[0];
            _methodInfo.Invoke(moduleInstance, parameters);
        }
    }
}
