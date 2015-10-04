using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CISanityTester.Template.Entities
{
    public class Build :BaseEntity
    {
        public List<Sanity> Sanities { get; set; }
        public string Branch { get; set; }
        public string Server { get; set; }
        public string P4CounterCmd { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


    }
}
