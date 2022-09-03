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

            // configurate server
            config.Server = config.GetServer();
            config.Server.Start();

            
        }
        // This function will be check list args and execute their
        static void OptionsChecker(ref string[] args)
        {

        }

        // This func wiil be init config file './gloval-config.json'
        static void Init()
        {

        }
    }
}
