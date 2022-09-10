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
