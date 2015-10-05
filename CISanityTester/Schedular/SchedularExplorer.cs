using CSSTerminal.GUIBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSTerminal.Schedular
{
    public class SchedularExplorer :ObjectTreeExplorer
    {
        public SchedularExplorer() : base()
        {
            AddTreeItem("Sanity Queue");
            AddTreeItem("Task List");
        }
    }
}
