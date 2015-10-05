
using CSSTerminal.GUIBase;
using CSSTerminal.Sessions;
using CSSTerminal.Template;
using CSSTerminal.Template.Entities.InfineraProductLine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSTerminal.InfineraProductExplorer
{
    public class EquipmentBaseController
    {
        public IPExplorer ipexplorer;
        public Template.Template template = null;
        protected BackgroundWorker bw_worker = null;
        protected TelnetSession NE_TL1 = null;

        public EquipmentBaseController(IPExplorer exp)
        {
            bw_worker = new BackgroundWorker();
            ipexplorer = exp;
            
        }
        public void SetCurrentTL1Session()
        {
            HierarchicalObjectViewModel model = (HierarchicalObjectViewModel)ipexplorer.ObjectTree.SelectedItem;
            var ne = from x in template.NEs where x.Name == model.Name select x;
            NetworkElement NE = ne.First();

            SessionExplorer sessexp = SessionExplorer.obj;
            // establish TL1 session with NE's XCM
            NE_TL1 =  sessexp.GetSession(NE.IP, 9090);
        }
    }
}
