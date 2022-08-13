// using System;
// using System.Text;
// using System.IO;
// using System.Net;
// using System.Net.Sockets;
// ////////////////////////////////////////////////////////
// string ip = "127.0.0.1";
// int port = 9000;

// FastCgi fcgi = new FastCgi(ip, port);
// fcgi.Open();


// ////////////////////////////////////////////////////////
// public class FastCgi
// {
//     public string Ip { get; set; }
//     public int Port { get; set; }
//     private Socket _socket;
//     private IPEndPoint _ip;

//     public FastCgi(string ip, int port)
//     {
//         this.Ip = ip;
//         this.Port = port;
//         _ip = new IPEndPoint(
//             IPAddress.Parse(ip),
//             (port + 1)
//         );
//         _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//         _socket.Bind(_ip);
//     }

//     public void Open()
//     {
//         byte[] head = {1, 1, 1, 8, 0};
//         /*
//             struct
//             {
//                 char protocolVersion;
//                 char type;
//                 char id;
//                 char contentLength;
//                 char alignment;
//             }
//         */
//         byte[] body = {1, 0};
//         /*
//             scruct
//             {
//                 char role;
//                 char flag;
//             }
//         */
//         _socket.Connect(
//             IPAddress.Parse(Ip), Port
//         );
//         _socket.Send(head);
//         _socket.Send(body);
//     }
// }
