using System;

namespace AMES
{
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Text.Encoding.RegisterProvider(
                System.Text.CodePagesEncodingProvider.Instance
            );

            Configurator config = new();
            config = Configurator.Deserialize(
                $@"{AppDomain.CurrentDomain.BaseDirectory}global-config.json"
                );

            // configurate server
            config.CheckedDirectories();
            config.Restart();
            config.Server = config.GetServer();
            config.Server.Start();
        }
    }
}
