using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortKnock
{
    public class PortUT
    {
        private bool tcp = false;
        private int port = 0;

        public bool Tcp { get => tcp; set => tcp = value; }
        public int Port { get => port; set => port = value; }

        public PortUT(int port, bool tcp = false)
        {
            this.port = port;
            this.tcp = tcp;
        }
    }
}
