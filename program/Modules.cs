using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        [JsonIgnore]
        public bool Enabled { 
            get { return _enabled; } 
            set { _enabled = value; } 
        }

        [JsonIgnore]
        public CancellationTokenSource CancellationToken {
            get { return _cts; }
            set { _cts = value; }
        }
        //private property/field
        private List<ModuleStruct> _dlls = new();
        private bool _enabled;
        private CancellationTokenSource _cts;

        // public methods
        public Modules() : base(isCollectible:true) { }
        public void Start()
        {
            if(_enabled && Active)
            {
                this.Init();
                this.WorkAsync(ModuleMode.Start);
                this.UpdateAsync(null);
            }
        }

        public void End()
        {
            this.WorkAsync(ModuleMode.End);
        }

        public void Begin()
        {
            this.WorkAsync(ModuleMode.Begin);
        }

        public void Stop()
        {
            _cts.Cancel();
            this.Unload();
        }

        public void Update()
        {
            this.UpdateAsync(null);
        }

        public void Init()
        {
            foreach (var obj in Dlls)
            {
                if (File.Exists(obj))
                {
                    this.Init2(obj);
                }
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + obj))
                {
                    string path = $"{AppDomain.CurrentDomain.BaseDirectory + obj}";
                    this.Init2(path);
                }
                else
                {
                    Console.WriteLine($"Module not found\nPath: {obj}");
                }
            }
        }
        // private methods
        private async void WorkAsync(ModuleMode mode)
        {
            await Task.Run(() => {
                foreach (var module in _dlls)
                {
                    if (module.Mode == mode) Task.Run(module.Activate);
                }
            }, _cts.Token
            );
        }

        private async void UpdateAsync(object obj)
        {
            await Task.Run(() => {
                while (!_cts.IsCancellationRequested)
                {
                    foreach (var module in _dlls)
                    {
                        if (module.Mode == ModuleMode.Update) module.Activate();
                    }
                }
            }, _cts.Token
            );
        }

        

        private void Init2(string path)
        {
            Assembly externModule = LoadFromAssemblyPath(path);
            Type classModule = externModule.GetType("Module");
            ModuleStruct module = new();
            module.Path = path;
            MethodInfo[] methods = classModule.GetMethods();
            foreach (var met in methods)
            {
                module.Path = path;
                switch(met.Name)
                {
                    case "Start":
                        module.Mode = ModuleMode.Start;
                    break;
                    case "Begin":
                        module.Mode = ModuleMode.Begin;
                    break;
                    case "Update":
                        module.Mode = ModuleMode.Update;
                    break;
                    case "End":
                        module.Mode = ModuleMode.End;
                    break;
                    Default:
                        module.Mode = ModuleMode.None;
                    break;
                }
                module.MethodInfo = met;
                _dlls.Add(module);
            }
        }
    }


    public struct ModuleStruct
    {
        //public propertys/fields
        [JsonIgnore]
        public string Path {
            get { return _path; }
            set { _path = value; }
        }
        [JsonIgnore]
        public MethodInfo MethodInfo {
            get { return _methodInfo; }
            set { _methodInfo = value; }
        }
        [JsonIgnore]
        public ModuleMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }
        //private [rp[ertys/fields
        private string _path;
        private MethodInfo _methodInfo;
        private ModuleMode _mode;

        public void Activate()
        {
            object moduleInstance = Activator.CreateInstance(_methodInfo.ReflectedType);
            object[] parameters = new object[0];
            _methodInfo.Invoke(moduleInstance, parameters);
            //try { } catch () { } finally { }
        }
    }


    public enum ModuleMode
    {
        None,
        Start,
        Begin,
        Update,
        End
    }
}
