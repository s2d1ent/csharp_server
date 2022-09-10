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
using System.Threading;
using System.Threading.Tasks;

namespace AMES
{
    internal class AMESLogger
    {
        public string Path;
        
        private Mutex _mutex;        

        public AMESLogger()
        {
            _mutex = new();
        }

        public void SetLog(AMESModuleType amesModule, string message)
        {
            _mutex.WaitOne();
            try
            {
                if(Path == null || Path == "")
                {
                    Path = Constants.PATH + $"logs/{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}.log.txt";
                }

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
               _mutex.ReleaseMutex();
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
