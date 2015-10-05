using CISanityTester.GUIBase;
using CISanityTester.Template;
using CISanityTester.Template.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace CISanityTester.Sessions
{
    public class SessionExplorer :ObjectTreeExplorer
    {
        public static SessionExplorer obj=null;
        private Template.Template template = null;
        public List<TelnetSession> SessionList { get; set; }
        private LayoutAnchorable m_layoutanch;
        public SessionExplorer():base()
        {
            SessionList = new List<TelnetSession>();
            ObjectTree.MouseDoubleClick += ObjectTree_MouseDoubleClick;
           
           // LoadSessionsFromTemplate();
            obj = this;
        }
        
        internal void ShowExplorer()
        {
            if (m_layoutanch == null)
            {
                m_layoutanch = new LayoutAnchorable() { Title = "Server Sessions" };
                m_layoutanch.Hiding += ExplorerClosed;
            }

            if(m_layoutanch.Content==null)
                m_layoutanch.Content = this;

           LayoutAnchorablePane anchpane =   MainWindow.GetMainWindow().GetLayoutAnchorablePane();
            if(!anchpane.Children.Contains(m_layoutanch))
                anchpane.Children.Add(m_layoutanch);
        }

        private void ExplorerClosed(object sender, EventArgs e)
        {
            MainWindow.GetMainWindow().MenuButton_View_ServerSession.IsChecked = false;
        }

        internal void HideExplorer()
        {
            LayoutAnchorablePane anchpane = MainWindow.GetMainWindow().GetLayoutAnchorablePane();

            if (m_layoutanch != null)
            {
                m_layoutanch.Content = null;
                if (anchpane.Children.Contains(m_layoutanch))
                    anchpane.Children.Remove(m_layoutanch);

                if (anchpane.Children.Count == 0)
                    anchpane.Parent.RemoveChild(anchpane);
            }
        }
        private void ObjectTree_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SelectedSession.Open();
        }
        protected override void Toolbar_Add_Button_Click(object sender, RoutedEventArgs e)
        {
            base.Toolbar_Add_Button_Click(sender, e);
            SessionProperty dlg = new SessionProperty();
            dlg.ShowDialog();
            if (dlg.telnetsession != null)
            {
                AddSession(dlg.telnetsession,true);

                template.ServerSessions.Add(new ServerSession() {Name=dlg.telnetsession.SessionName, ServerIP = dlg.telnetsession.ServerIP,  Port = dlg.telnetsession.ServerPort  });
            }
        }

       
        public void LoadSessionsFromTemplate()
        {
            template = TemplateFileHandler.GetTemplateFileHandler().CurrentTemplate;
            foreach (ServerSession session in template.ServerSessions)
            {
                // Start the session if LoadOnstartup is true
               
                    AddSession(session.ServerIP,  Convert.ToInt32(session.Port), session.Name, session.LoadOnStartup,session.SessionProtocol);
            }
        }

        private void Session_DeleteMenu_Click(object sender, RoutedEventArgs e)
        {
            HierarchicalObjectViewModel obj = (HierarchicalObjectViewModel) ObjectTree.SelectedItem;
            if (MessageBoxResult.Yes == MessageBox.Show("Are You really want to delete session \"" + obj.Name + "\"", "Delete Confirm", MessageBoxButton.YesNo)) 
                DeleteSession(GetSession(obj.Name));
        }

        private void Session_LogFile_Click(object sender, RoutedEventArgs e)
        {
            
            //System.Diagnostics.Process.Start(SelectedSession.logfile.LogFilePathWithFile);
        }
        private void Session_LogDirectory_Click(object sender, RoutedEventArgs e)
        {
            
            //System.Diagnostics.Process.Start(SelectedSession.logfile.LogDirectory);
        }

        private void Session_EditMenu_Click(object sender, RoutedEventArgs e)
        {
                     
            SessionProperty dlg = new SessionProperty(SelectedSession);
            dlg.ShowDialog();

        }

        internal void CloseAllSessions()
        {
            foreach(TelnetSession session in SessionList)
            {
                DeleteTreeItem(session.SessionName);
                session.Dispose();
            }
            SessionList.Clear();
            ReloadTree();
        }

       
        internal TelnetSession GetSession(string ip, int port)
        {
            var list = from session in SessionList where session.ServerIP == ip && session.ServerPort == port select session;
            if (list.Count() != 0)
                return list.First();

            return AddSession(ip, port, ip + ":" + port, true,"TL1");

        }
        internal TelnetSession GetSession(string name)
        {
            var list = from session in SessionList where session.SessionName == name select session;
            if (list.Count() != 0)
                return list.First();

            return null;

        }
        public TelnetSession SelectedSession {
            get {
                HierarchicalObjectViewModel model = (HierarchicalObjectViewModel)ObjectTree.SelectedItem;
                var list = from session in SessionList where session.SessionName == model.Name select session;
                if (list.Count() == 0)
                    return null;
                return list.First<TelnetSession>();
            }
        }

        private TelnetSession AddSession(string ip, int port,string name, bool openinlayoutdocument, string sessionprotocol)
        {
            
            if (name == "")
            {
                name = ip + ":" + port;
            }
            TelnetSession telsession = new TelnetSession(ip, port,name,sessionprotocol, openinlayoutdocument,true,true);
            AddSession(telsession,openinlayoutdocument);

            return telsession;
        }
        private void AddSession(TelnetSession session,bool openinlayoutdocument)
        {
            HierarchicalObjectViewModel tmp = AddTreeItem(session.SessionName);
            tmp.Image = @"..\Resources\Images\ServerSession\terminal.png";
            //Create ContextMenu of Build
            ContextMenu cm = new ContextMenu();
            MenuItem itm = new MenuItem() { Header = "Edit" };
            itm.Click += new RoutedEventHandler(Session_EditMenu_Click);
            cm.Items.Add(itm);

            itm = new MenuItem() { Header = "Log File" };
            itm.Click += new RoutedEventHandler(Session_LogFile_Click);
            cm.Items.Add(itm);

            itm = new MenuItem() { Header = "Log Directory" };
            itm.Click += new RoutedEventHandler(Session_LogDirectory_Click);
            cm.Items.Add(itm);

            itm = new MenuItem() { Header = "Delete" };
            itm.Click += new RoutedEventHandler(Session_DeleteMenu_Click);
            cm.Items.Add(itm);
            tmp.ContextMenuObj = cm;

            
            SessionList.Add(session);
            ReloadTree();
        }
       
        private void DeleteSession(TelnetSession session)
        {
            var serversessions = from s in template.ServerSessions where s.Name == session.SessionName select s;
            if (serversessions.Count() == 0)
                return;

            template.ServerSessions.Remove(serversessions.First());
            SessionList.Remove(session);
            DeleteTreeItem(session.SessionName);
            session.Dispose();
            ReloadTree();
            
        }
        private void DeleteSession(string sessionname)
        {
            DeleteSession(GetSession(sessionname));
        }
       
    }
}

