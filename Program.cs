using System;

namespace AMES
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string configPath = $@"{AppDomain.CurrentDomain.BaseDirectory}global-config.json";
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

            // configurate server
            config.Server = config.GetServer();
            config.Server.Start();

            
        }
    }
}
