using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Program
{
    internal class Python : Interpreter
    {
        public Python() { }
        public Python (Response response) : base(response)
        {
            
        }
    }
}
