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

namespace AMES
{
    internal class Program
    {
        static private string configPath = $@"{AppDomain.CurrentDomain.BaseDirectory}global-config.json";
        static void Main(string[] args)
        {
            OptionsChecker(ref args);
            
            System.Text.Encoding.RegisterProvider(
                System.Text.CodePagesEncodingProvider.Instance
            );

            Configurator config = new();
            config = Configurator.Deserialize(
                    configPath
                );
            config.CheckedDirectories();

            // RApi
            //config.RemoteApi.StartAsync();

            // configurate server (single mode)
            if(config.ServerMode != ServerMode.Container)
            {
                config.Server = config.GetServer();
                config.Server.Start();
            }
            else
            {

            }

            
        }
        // This function will be check args list and execute their
        static void OptionsChecker(ref string[] args)
        {

        }

        // This func wiil be init config file './gloval-config.json'
        static void Init()
        {

        }
    }
}
