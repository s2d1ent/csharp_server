using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Program
{
    internal class Php : Interpreter
    {
        public Php() { }
        public Php(Response response) : base(response)
        {

        }
    }
}
