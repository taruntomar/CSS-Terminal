using CSSTerminal.Sessions.TelnetSessionNS.TarminalLayoutDocumentNS.TerminalNS;
using System;
using System.Windows.Media.Imaging;
using Xceed.Wpf.AvalonDock.Layout;

namespace CSSTerminal.Sessions.TelnetSessionNS.TermianlLayoutDocumentNS
{
    public class TermianlLayoutDocument : LayoutDocument, IDisposable
    {
        
        public bool IsOpen { get; set; }

        private TerminalBox TerminalBoxObj = null;

        public TermianlLayoutDocument(TelnetSession session,bool isopen, bool LogFileEnable)
        {
            TerminalBoxObj = new TerminalBox(session, LogFileEnable);
            this.Content = TerminalBoxObj;
            Title = session.SessionName;
            ToolTip = session.ServerIP + ":" + session.ServerPort;
            IconSource = new BitmapImage(new Uri(@"\Resources\Images\ServerSession\DisSessionIcon.png",UriKind.RelativeOrAbsolute));

            if (isopen)
                MainWindow.GetMainWindow().AddServerSession(this);

            IsOpen = isopen;
        }
        

        public void Open()
        {
            if (IsOpen) {
                this.IsSelected = true;
                return;
            }

              

            MainWindow.GetMainWindow().AddServerSession(this);
            IsOpen = true;
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            MainWindow.GetMainWindow().RemoveServerSession(this);
            IsOpen = false;
        }
       
       
        public void Connect()
        {
            TerminalBoxObj.Connect();
        }

        public void Write(string str)
        {
            TerminalBoxObj.Write(str);          
        }

        public void Dispose()
        {
            
            TerminalBoxObj.Dispose();
        }
    }
}
