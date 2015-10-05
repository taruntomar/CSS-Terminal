using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSTerminal.Template.Entities
{
    public class Sanity : BaseEntity
    {
        public List<TestScript> Scripts { get; set; }
    }
}
