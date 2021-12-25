using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Threading;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            // add new encodings
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Global global;
            if (File.Exists($@"{path}/global-config.json"))
                global = SerialaizeGlobal($@"{path}/global-config.json");
            else
            {
                Console.WriteLine("global-config.json not found");
                return;
            }

            // min&max thread will be used
            ThreadPool.SetMinThreads(global.MinWorker, global.MinWorkerAsync);
            ThreadPool.SetMinThreads(global.ManWorker, global.ManWorkerAsync);

            // begin dlls start
            if(global.Modules != null && global.ModuleEnabled)
            {
                global.Modules.Init();
                global.Modules.Begin();
            }

            // get server
            global.GetServer();
            Console.WriteLine(global.GetInfo());

            // start
            global.Server.StartAsync();
            
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
                        ThreadPool.SetMinThreads(global.MinWorker, global.MinWorkerAsync);
                        ThreadPool.SetMinThreads(global.ManWorker, global.ManWorkerAsync);
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
                    cmd_bool = false;
                }
                if(cmd == "exit" || cmd == "-e" && cmd_bool)
                {
                    global.Server.Stop();
                    //global.SerializeConfig();

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
            string json = File.ReadAllText(address);
            json = json.Replace("\\", "/");
            result = JsonSerializer.Deserialize<Global>(json);
            bool save = false;
            if(result.MinWorker == 1 || result.MinWorker < 0)
            {
                result.MinWorker = 2;
            }

            if (result.MinWorkerAsync == 1 || result.MinWorkerAsync < 0)
            {
                result.MinWorkerAsync = 2;
            }

            if (result.ManWorker == 1 || result.ManWorker < 0)
            {
                result.ManWorker = 2;
            }

            if (result.ManWorkerAsync == 1 || result.ManWorkerAsync < 0)
            {
                result.ManWorkerAsync = 2;
            }

            if (result.ListenUse == null) result.ListenUse = false;

            if (result.MultipleSite == null) result.MultipleSite = true;

            if (result.ModuleEnabled == null) result.ModuleEnabled = false;

            if (result.Server == null)
            {
                result.Server = new();
                result.Server.Extensions = new string[] { ".py", ".php", ".xhtml", ".html", ".html"  };
                save = true;
            }

            if(result.MySqlPath == null)
            {
                result.MySqlPath = new string[] {
                    "includes/mysql/bin/mysqld.exe",
                    "includes/mysql/bin/mysqladmin.exe",
                    "includes/mysql/bin/mysql.exe"
                };
                save = true;
            }

            if(result.Interpreters == null)
            {
                Interpreter php = new();
                php.Version = "x86";
                php.Name = "php";
                php.Path = "includes/php/php.exe";
                php.Type = "int";

                Interpreter phpcgi = new();
                php.Version = "x86";
                php.Name = "php";
                php.Path = "includes/php/php-cgi.exe";
                php.Type = "cgi";

                Interpreter python = new();
                php.Version = "x86";
                php.Name = "py";
                php.Path = "includes/python/win86/python.exe";
                php.Type = "int";

                result.Interpreters = new System.Collections.Generic.Dictionary<string, Interpreter>() {
                    { "php", php },
                    { "phpcgi", phpcgi },
                    { "python", python }
                };
                save = true;
            }
            if (save)
                result.SerializeConfig();
            return result;
        }
    }
}
