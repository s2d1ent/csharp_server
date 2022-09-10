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
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMES
{
    internal class Php : Interpreter
    {
        public byte[] GetFile(string path)
        {
            byte[] result = new byte[1024];
            string err = "";
            string data = "";
            
            ProcessStartInfo info = new ProcessStartInfo(Constants.PATH_PHP, path);
            info.UseShellExecute = false;
            info.ErrorDialog = false;
            info.RedirectStandardError = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.CreateNoWindow = true;
            Process p = new Process();
            p.StartInfo = info;


            bool pStarted = p.Start();

            StreamWriter input = p.StandardInput;
            StreamReader output = p.StandardOutput;
            StreamReader error = p.StandardError;

            if(output.CurrentEncoding != Encoding.UTF8)
            {
                err = error.ReadToEnd();
                data = output.ReadToEnd();
                err += data;
                byte[] buffer = output.CurrentEncoding.GetBytes(err);
                result = Encoding.Convert(Encoding.GetEncoding(866), Encoding.UTF8, buffer);

            }
            else
            {
                err = error.ReadToEnd();
                data = output.ReadToEnd();
                err += data;
                result = output.CurrentEncoding.GetBytes(err);  
            }

            return result;
        }
    }
}
