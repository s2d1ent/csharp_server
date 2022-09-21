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

/*
    Documentation FastCGI - https://fastcgi-archives.github.io/FastCGI_Specification.html#S1
*/

using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

class Program1
{
    static void main()
    {
        string ip = "127.0.0.1";
        int port = 9000;

        FCGI fcgi = new FCGI(ip, port);
        fcgi.Open();
    }
}

public class FCGI
{
    public string Ip { get; set; }
    public int Port { get; set; }
    private Socket _socket;
    private IPEndPoint _ip;

    public FCGI(string ip, int port)
    {
        this.Ip = ip;
        this.Port = port;
        _ip = new IPEndPoint(
            IPAddress.Parse(ip),
            (port + 1)
        );
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.Bind(_ip);
    }

    public void Open()
    {
        
    }
}

struct FCGI_Record{
    char version;
    char type;
    char requestIdB1;
    char requestIdB0;
    char contentLengthB1;
    char contentLengthB0;
    char paddingLength;
    char reserved;
    char[] contentData;
    char[] paddingData;
}