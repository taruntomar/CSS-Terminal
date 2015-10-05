using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSTerminal.Template.Entities
{
    public class ServerSession :BaseEntity
    {
        public string ServerIP { get; set; }
        public int Port { get; set; }
        public bool LoadOnStartup  { get; set; }
        public string SessionProtocol { get; set; }

    }
}
