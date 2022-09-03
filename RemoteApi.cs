using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AMES
{
    internal class RemoteApi
    {
        public string IP { get; set;}
        public ushort Port { get; set; }
        public bool Active;
        public RemoteApiType Type;
        private Socket _listener;
        private IPEndPoint _ip;
        private volatile CancellationTokenSource _cts;
        public RemoteApi()
        {
             _cts = new CancellationTokenSource();
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public RemoteApi(string ip)
        {
            IP = ip;
            Port = 1000;
            _ip = new IPEndPoint(
                IPAddress.Parse(ip),
                Port
            );
            _cts = new CancellationTokenSource();
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public RemoteApi(string ip, ushort port)
        {
            IP = ip;
            Port = port;
            _ip = new IPEndPoint(
                IPAddress.Parse(ip),
                port
            );
            _cts = new CancellationTokenSource();
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start()
        {
            if(!Active)
            {
                Active = !Active;
                if(_ip == null)
                {
                    _ip = new IPEndPoint(
                        IPAddress.Parse(IP),
                        Port
                    );
                }
                _listener.Bind(_ip);
                _listener.Listen();

                while(Active && !_cts.IsCancellationRequested)
                {
                    Socket client = _listener.Accept();
                    if(client == null)
                    {
                        continue;
                    }
                    Task.Run(
                        ()=>{Decoder(client);},
                        _cts.Token
                    );
                }
            }
            else
            {
                Console.WriteLine("Remote API уже активен");
            }
        }

        public async void StartAsync()
        {
            await Task.Run(
                ()=>{Start();}
            );
        }

        // [CONFIG]

        public void Decoder(Socket client)
        {
            string query = "";
            int size = client.ReceiveBufferSize;
            byte[] buffer = new byte[size];

            client.Receive(buffer);
            query = Encoding.UTF8.GetString(buffer);

            Console.WriteLine(query);
        }

    }

    internal enum RemoteApiType
    {
        privilege = 0,
        user = 1
    }
}
