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

        //private property/field
        private List<ModuleStruct> _dlls = new();
        private bool _enabled;

        // public methods
        public Modules() : base(isCollectible:true) { }
        public void Start()
        {
            this.Init();
            this.WorkModuleAsync(ModuleMode.Start);
            this.UpdateModuleAsync(null);
        }

        public void End()
        {
            this.WorkModuleAsync(ModuleMode.End);
        }

        public void Begin()
        {
            this.WorkModuleAsync(ModuleMode.Begin);
        }

        public void Stop()
        {
            this.Unload();
        }


        // private methods
        private async void WorkModuleAsync(ModuleMode mode)
        {
            await Task.Run(() => {
                foreach (var module in _dlls)
                {
                    if (module.Mode == mode) Task.Run(module.Activate);
                }
            });
        }
        private async void UpdateModuleAsync(object obj)
        {
            await Task.Run(()=> {
                TimerCallback callback = new TimerCallback(UpdateModuleAsync);
                Timer timer = new Timer(callback, null, 500, 0);
                foreach (var module in _dlls)
                {
                    if (module.Mode == ModuleMode.Update) Task.Run(module.Activate);
                }
            });
        }

        private void Init()
        {
            if(Dlls.Length != 0 && _enabled)
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
        }

        private void Init2(string path)
        {
            Assembly externModule = this.LoadFromAssemblyPath(path);
            Type classModule = externModule.GetType("Module");
            ModuleStruct module = new();
            module.Path = path;
            MethodInfo[] methods = classModule.GetMethods();
            foreach (var met in methods)
            {
                module.Path = path;
                if (met.Name == "Start")
                {
                    module.Mode = ModuleMode.Start;
                    module.MethodInfo = met;
                    _dlls.Add(module);
                }
                if (met.Name == "Begin")
                {
                    module.Mode = ModuleMode.Begin;
                    module.MethodInfo = met;
                    _dlls.Add(module);
                }
                if (met.Name == "Update")
                {
                    module.Mode = ModuleMode.Update;
                    module.MethodInfo = met;
                    _dlls.Add(module);
                }
                if (met.Name == "End")
                {
                    module.Mode = ModuleMode.End;
                    module.MethodInfo = met;
                    _dlls.Add(module);
                }
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
