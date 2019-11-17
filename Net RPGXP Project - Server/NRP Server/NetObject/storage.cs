using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRP_Server.NetObject
{
    class storage
    {
        public int id { get; private set; }
        public Dictionary<Item, int> inventory { get; private set; } = new Dictionary<Item, int>();

    }
}
