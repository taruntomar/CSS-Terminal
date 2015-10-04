using CISanityTester.Sessions;
using CISanityTester.Template;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
using System;
using System.Windows.Forms;

namespace CISanityTester
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

            //TemplateFileHandler.GetTemplateFileHandler().GenetareDefaultTemplate();
            //SessionExplorer.LoadSessionsFromTemplate();
            //IPExplorer.LoadIPFromTemplate();

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

        private void OpenTemplateMenuButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if(System.Windows.Forms.DialogResult.OK == dlg.ShowDialog())
            {
                CloseTemplateMenuButton_Click(null, null); //clsoe the old template before opening new template
                TemplateFileHandler.GetTemplateFileHandler().SetTemplate(dlg.FileName);
                SessionExplorer.LoadSessionsFromTemplate();
                IPExplorer.LoadIPFromTemplate();
            }


            
        }

        private void CloseTemplateMenuButton_Click(object sender, RoutedEventArgs e)
        {
            SessionExplorer.CloseAllSessions();
            IPExplorer.Close();
        }

        private void SaveTemplateMenuButton_Click(object sender, RoutedEventArgs e)
        {
            TemplateFileHandler.GetTemplateFileHandler().Save();
        }
    }
}
