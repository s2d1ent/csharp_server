using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AMES
{
    internal class AMESLogger
    {
        public string Path { get; set; }
        
        private Mutex _mutex;
        private Semaphore _semaphore;

        public AMESLogger()
        {
            Path = Constants.PATH + $"logs/{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}.log.txt";
            _mutex = new();
            _semaphore = new(1,1);
        }

        public void SetLog(AMESModuleType amesModule, string message)
        {
            _semaphore.WaitOne();
            try
            {
                TimeSpan dateTime = DateTime.Now.TimeOfDay;
            
                if(!File.Exists(Path))
                {
                    File.Create(Path);    
                }
                message = message + "\n";
                string append = $"[{dateTime}]:{amesModule.ToString()} {message}";
                byte[] buffer = Encoding.Default.GetBytes(append);

                using(FileStream fs = new FileStream(Path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 1024, true)) 
                {
                    fs.Write(buffer, 0, buffer.Length);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Logger " + ex.Message);
            }
            finally
            {
               _semaphore.Release();
            }
        }
    }

    internal enum AMESModuleType
    {
        Interpreter,
        Server,
        Client,
        Modules,
        Configurator,
        RemoteApi,
        ClientRapi
    }
}
