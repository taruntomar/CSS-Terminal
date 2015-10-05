using CSSTerminal.Sessions;
using CSSTerminal.Template;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
using System;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;

namespace CSSTerminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        //  The NotifyIcon object
        private System.Windows.Forms.NotifyIcon notifyIcon1;

        static MainWindow mainwindowobj = null;
        LayoutAnchorablePaneGroup layoutanchpanegroup;
        SessionExplorer m_sessionexporer = null;
        private LayoutAnchorablePane layoutanchpane;
        private List<TelnetSession> TempSessions;
        public MainWindow()
        {
            //TemplateFileHandler.GetTemplateFileHandler().GenetareDefaultTemplate();
            mainwindowobj = this;
            notifyIcon1 = new System.Windows.Forms.NotifyIcon();

            notifyIcon1.Icon = new Icon(@"..\..\Resources\Images\Application\Logo\logo.ico");

            notifyIcon1.ContextMenu = new System.Windows.Forms.ContextMenu();
            notifyIcon1.ContextMenu.MenuItems.Add("Exit", new System.EventHandler((object obj, System.EventArgs e) => { App.Current.Shutdown();  }));
          
            notifyIcon1.DoubleClick += NotifyIcon1_DoubleClick;
            InitializeComponent();
           
            m_sessionexporer = new SessionExplorer();
            

        }

        internal LayoutAnchorablePane GetLayoutAnchorablePane()
        {
            if (layoutanchpanegroup == null)
            {
                layoutanchpanegroup = new LayoutAnchorablePaneGroup();
                layoutanchpanegroup.DockWidth = new GridLength(250);
            }

            if (layoutanchpane == null)
                layoutanchpane = new LayoutAnchorablePane();

            if (!layoutanchpanegroup.Children.Contains(layoutanchpane))
                layoutanchpanegroup.Children.Add(layoutanchpane);

            if (!LayoutPane.Children.Contains(layoutanchpanegroup))
            LayoutPane.Children.Add(layoutanchpanegroup);

            return layoutanchpane;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            notifyIcon1.Dispose();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
            notifyIcon1.BalloonTipTitle = "CI Sanity Tester";
            notifyIcon1.BalloonTipText = "Running in background..";

            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(500);
            this.Hide();
            
        }


        public static MainWindow GetMainWindow()
        {
            return mainwindowobj;
        }
        public void AddServerSession(LayoutDocument terminallayoutdoc)
        {
            LayoutDocumentPane.Children.Add(terminallayoutdoc);
        }
        public void RemoveServerSession(LayoutDocument terminallayoutdoc)
        {
            LayoutDocumentPane.Children.Remove(terminallayoutdoc);
        }

        private void NotifyIcon1_DoubleClick(object sender, System.EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Maximized;
        }

        private void UpdateTitle()
        {
            Title = AssemblyProduct + " - " + TemplateFileHandler.GetTemplateFileHandler().CurrentTemplate.Name;
        }
        

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        private void CloseTemplateMenuButton_Click(object sender, RoutedEventArgs e)
        {
            m_sessionexporer.CloseAllSessions();
            m_sessionexporer.HideExplorer();
            TemplateFileHandlerButtonSwitch(false);
            //IPExplorer.Close();
        }

 
        private void TemplateFileHandlerButtonSwitch(bool value)
        {

            SaveTemplateMenuButton.IsEnabled = value;
            SaveAsTemplateMenuButton.IsEnabled = value;
            CloseTemplateMenuButton.IsEnabled = value;
            SaveButton.IsEnabled = value;
            CloseButton.IsEnabled = value;

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new ApplicationAboutBox().ShowDialog();
        }

 

        private void SwitchServerSessionExplorer(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem menuitem = (System.Windows.Controls.MenuItem)sender;
            if (menuitem.IsChecked == false && m_sessionexporer != null)
                m_sessionexporer.HideExplorer();
            else if (menuitem.IsChecked == true && m_sessionexporer != null)
                m_sessionexporer.ShowExplorer();
        }

        #region Cammands

        private void CMD_Open_Template(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (System.Windows.Forms.DialogResult.OK == dlg.ShowDialog())
            {
                CloseTemplateMenuButton_Click(null, null); //clsoe the old template before opening new template
                TemplateFileHandler.GetTemplateFileHandler().SetTemplate(dlg.FileName);
                m_sessionexporer.LoadSessionsFromTemplate();
                //IPExplorer.LoadIPFromTemplate();
                UpdateTitle();
                m_sessionexporer.ShowExplorer();
                TemplateFileHandlerButtonSwitch(true);
            }

        }

        private void CMD_Save_Template(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            TemplateFileHandler.GetTemplateFileHandler().Save();
            UpdateTitle();
        }

        private void CMD_New_Template(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            CloseTemplateMenuButton_Click(null, null);
            TemplateFileHandler.GetTemplateFileHandler().GenetareDefaultTemplate();
            m_sessionexporer.LoadSessionsFromTemplate();
            m_sessionexporer.ShowExplorer();

            UpdateTitle();
            TemplateFileHandlerButtonSwitch(true);
            //IPExplorer.LoadIPFromTemplate();
        }
        #endregion

        #region MenuItemClickActions(SaveAs,Exit)
        private void SaveAsTemplateMenuButton_Click(object sender, RoutedEventArgs e)
        {
            TemplateFileHandler.GetTemplateFileHandler().SaveAs();
        }

        private void ExitApplicationMenuButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        #endregion

        private void QuickConnectToolBarButton_Click(object sender, RoutedEventArgs e)
        {
            string serverIp = Toolbar_QuickConnect_ServerIPTextBox.Text;
            string port = Toolbar_QuickConnect_ServerportTextBox.Text;
            string type = Toolbar_QuickConnect_ServerClientTextBox.Text;

            if (TempSessions == null)
                TempSessions = new List<TelnetSession>();

            TelnetSession session = new TelnetSession(serverIp, Convert.ToInt32(port), serverIp + ":" + port, type, true, false,false);
            session.Connect();
            TempSessions.Add(session);


        }
    }
}
