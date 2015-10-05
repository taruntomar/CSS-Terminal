using CSSTerminal.GUIBase;
using CSSTerminal.SanitySuitTree.BuildTreecontrol;
using CSSTerminal.Template;
using CSSTerminal.Template.Entities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CSSTerminal.SanitySuitTree
{
    class SanitySuitExplorer:ObjectTreeExplorer
    {
        Template.Template template = null;
        public SanitySuitExplorer() : base(){
            template = TemplateFileHandler.GetTemplateFileHandler().CurrentTemplate;
            AddBuilds();
        }
      
        void AddBuilds()
        {

            foreach (Build build in template.Builds)

            {
                HierarchicalObjectViewModel tmp = AddTreeItem(build.Name);
                
                //Create ContextMenu of Build
                ContextMenu cm = new ContextMenu();
                MenuItem itm = new MenuItem() { Header = "Edit" };
                itm.Click += new RoutedEventHandler(Build_EditMenu_Click);
                cm.Items.Add(itm);


                itm = new MenuItem() { Header = "Delete" };
                itm.Click += new RoutedEventHandler(Build_DeleteMenu_Click);
                cm.Items.Add(itm);
                tmp.ContextMenuObj = cm;


             
                AddSanities(tmp, build.Sanities);
            }
        }
        private void Build_EditMenu_Click(object sender, RoutedEventArgs e)
        {

            BuildControl buildcontrol = new BuildControl();
            bool change = buildcontrol.OpenPropertyWindowDialog();
            
        }



        private void Build_DeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }



        void AddSanities(HierarchicalObjectViewModel model, List<Sanity> list)
        {
            foreach (Sanity obj in list)
            {
                HierarchicalObjectViewModel tmp = new HierarchicalObjectViewModel();
                tmp.Name = obj.Name;
                model.HierarchicalObjects.Add(tmp);
                AddTestScripts(tmp, obj.Scripts);
            }
        }
        void AddTestScripts(HierarchicalObjectViewModel model, List<TestScript> list)
        {
            foreach (TestScript obj in list)
            {
                HierarchicalObjectViewModel tmp = new HierarchicalObjectViewModel();
                tmp.Name = obj.Name;
                model.HierarchicalObjects.Add(tmp);

            }
        }
    }
}
