using CSSTerminal.GUIBase;
using System.Windows.Input;
using System;

namespace CSSTerminal.InfineraProductExplorer
{
    public class IPExplorer:ObjectTreeExplorer
    {
        NE_Control necontrol = null;
        public IPExplorer() : base()
        {
            necontrol = new NE_Control(this);
            //LoadIPFromTemplate();
        }
        protected override void TreeObjectRightClick(object sender, MouseButtonEventArgs e)
        {
            base.TreeObjectRightClick(sender, e);
            necontrol.SetCurrentTL1Session();
        }

        internal void LoadIPFromTemplate()
        {
            necontrol.LoadNEs();
        }

        internal void Close()
        {
            
        }

      
    }
}
