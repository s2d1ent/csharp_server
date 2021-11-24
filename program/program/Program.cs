using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace program
{
    class Program
    {
        /**/
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Global global = SerialaizeGlobal($@"{path}/global-config.json");
            // min&max thread will be used
            ThreadPool.SetMinThreads(global.ThreadPoolMin_worker, global.ThreadPoolMin_async);
            ThreadPool.SetMinThreads(global.ThreadPoolMax_worker, global.ThreadPoolMax_async);
            // get server
            global.GetServer();
            Console.WriteLine(global.GetInfo());
            // start
            global.Server.StartAsync();
            global.MySqlServerStart();
            while (true)
                {
                string cmd = Console.ReadLine();
                bool cmd_bool = true;
                if (cmd.IndexOf("help") != -1 && cmd_bool)
                {
                    string help_path = $"{AppDomain.CurrentDomain.BaseDirectory}/help";
                    if (cmd == "help")
                        Console.WriteLine($"{File.ReadAllText($"{help_path}/{cmd}")}");

                    cmd_bool = false;
                }
                if(cmd.IndexOf("mysql") != -1)
                {
                    if(cmd == "mysql")
                    {
                        Console.WriteLine($"{File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}/mysql")}");
                    }
                    if(cmd.IndexOf("shell") != -1)
                    {
                        if(global.Server.Active)
                        {
                            Interpreter open = new Interpreter();
                            open.OpenApplication($"{AppDomain.CurrentDomain.BaseDirectory}{global.MySql_path[2]}", "-u root");
                            //open.OpenApplication($@"C:\Windows\System32\cmd.exe", "");
                        }
                    }
                    if (cmd.IndexOf("start") != -1)
                    {
                        global.MySqlServerStart();
                    }
                    if (cmd.IndexOf("stop") != -1)
                    {
                        global.MySqlServerClose();
                    }
                    if (cmd.IndexOf("restart") != -1)
                    {
                        global.MySqlServerClose();
                        global.MySqlServerStart();
                    }
                    cmd_bool = false;
                }
                if (cmd == "clear" || cmd == "-c" && cmd_bool)
                {
                    Console.Clear();
                    cmd_bool = false;
                }
                if(cmd == "start" || cmd == "-st" && cmd_bool)
                {
                    global.Server.StartAsync();
                    //global.MySqlServerStartAsync();
                    cmd_bool = false;
                }
                if(cmd.IndexOf("alias") != -1 && cmd_bool)
                {
                    if(cmd == "alias")
                    {
                        Console.WriteLine($"{File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}/help/{cmd}")}");
                    }
                    if(cmd.IndexOf("add") != -1)
                    {
                        string folder = "", alias = "";
                        bool error = false;
                        try
                        {
                            string f = Regex.Match(cmd, @"^\w[a-z]+\s").Value;
                            cmd = cmd.Replace(f, "");
                            f = Regex.Match(cmd, @"^\w[a-z]+\s").Value;
                            cmd = cmd.Replace(f, "");
                            folder = Regex.Match(cmd, @"^\w[a-z]+").Value;
                            alias = Regex.Match(cmd, @"\w[a-z]+$").Value;
                            if (folder == "" || folder == " ")
                                error = true;
                            if(Directory.Exists($"{AppDomain.CurrentDomain.BaseDirectory}{global.Server.Path}/{folder}") && !error)
                            {
                                Console.WriteLine($"{AppDomain.CurrentDomain.BaseDirectory}{global.Server.Path}/{folder}");
                                error = false;
                            }
                        }
                        catch(Exception ex)
                        {
                            error = true;
                        }
                        finally
                        {
                            if (folder == alias || folder == null || folder == "" || alias == null || alias == "" && error)
                            {

                                Console.Write($"Uncorrect input data: ");
                                Console.WriteLine($"alias add [folder] [name]\n");
                            }
                            
                            if (!error)
                            {
                                global.Alias.Add(folder, alias);
                                global.SerializeConfig();
                            }
                        }
                    }
                    if(cmd.IndexOf("show") != -1 && cmd_bool)
                    {
                        Console.WriteLine($"Alias list:");
                        string list = "";
                        foreach(var i in global.Alias)
                        {
                            list += $"Folder: {i.Key} Alias: {i.Value}\n";
                        }
                        Console.WriteLine(list);
                    }
                    if(cmd.IndexOf("remove") != -1 || cmd.IndexOf("-rm") != -1 && cmd_bool)
                    {
                        cmd = cmd.Replace("alias remove ", "").Replace("alias -rm " , "")
                            .Replace(" ", "");
                        string folder = cmd;
                        bool error = false;
                        if (folder == null || folder == "" || folder == " ")
                        {
                            Console.Write($"Uncorrect input data: ");
                            Console.Write($"alias remove[-rm] [folder] [name]\n");
                            error = true;
                        }
                        if(!error)
                        {
                            global.Alias.Remove(folder);
                            global.SerializeConfig();
                        }
                    }
                    cmd_bool = false;
                    //cmd = "restart";
                }
                if (cmd.IndexOf("restart") != -1 && cmd_bool)
                {
                    if (cmd == "restart")
                    {
                        Console.Clear();
                        global.MySqlServerClose();
                        global.Server.Stop();
                        global = SerialaizeGlobal($@"{path}/global-config.json");
                        ThreadPool.SetMinThreads(global.ThreadPoolMin_worker, global.ThreadPoolMin_async);
                        ThreadPool.SetMinThreads(global.ThreadPoolMax_worker, global.ThreadPoolMax_async);
                        global.GetServer();
                        Console.WriteLine(global.GetInfo());
                        global.Server.StartAsync();
                        global.MySqlServerStartAsync();
                    }
                    cmd_bool = false;
                }
                if (cmd == "stop"  && cmd_bool)
                {
                    global.Server.Stop();
                    //global.MySqlServerCloseAsync();
                    cmd_bool = false;
                }
                if(cmd == "exit" || cmd == "-e" && cmd_bool)
                {
                    //global.MySqlServerClose();
                    global.Server.Stop();
                    //global.SerializeConfig();
                    cmd_bool = false;

                    return;
                }
                if(cmd.IndexOf("server") != -1 && cmd_bool)
                {
                    if(cmd == "server")
                    {
                        Console.WriteLine(global.GetInfo());
                    }
                    if(cmd.IndexOf("--status") != -1)
                    {
                        Console.WriteLine(global.Server.GetStatus());
                    }
                    cmd_bool = false;
                }
                if(cmd_bool)
                {
                    Console.WriteLine($"Command not found use help or [command] --help\n");
                }
            }
        }
        static Global SerialaizeGlobal(string address)
        {
            Global result = new Global();
            try
            {
                string json = File.ReadAllText(address);
                json = json.Replace("\\", "/");
                result = JsonSerializer.Deserialize<Global>(json);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return result;
        }
    }
}
