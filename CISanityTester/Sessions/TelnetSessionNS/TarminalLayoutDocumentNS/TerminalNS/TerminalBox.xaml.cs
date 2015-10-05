using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using TelnetExpect;

namespace CSSTerminal.Sessions.TelnetSessionNS.TarminalLayoutDocumentNS.TerminalNS
{
    /// <summary>
    /// Interaction logic for TerminalBox.xaml
    /// </summary>
        

public partial class TerminalBox : UserControl,IDisposable
    {
        private TelnetSession Session = null;
        private NetworkStream Stream = null;
        private TcpClient tcpclient = null;
        private bool _bw_Stop_Flag = false;
        private BackgroundWorker _bw_readstream = null;
        private LogFile logfile = null;
        private bool LogFileEnable = false;
        public TerminalBox(TelnetSession session,bool LogFileEnable)
        {
            Session = session;
            
            InitializeComponent();
            RichTextBox.SetTelnetSession(session);
            RichTextBox.WriteInStream += Write;
            //RichTextBox.UpdateCaret += UpdateCaret;
            SetupTextBox();
            this.LogFileEnable = LogFileEnable;
            if(LogFileEnable)
            logfile = new LogFile(session);

        }

        private void SetupTextBox()
        {
            // textbox. 
            // set onkeyinout event
            // set on textinput event

            RichTextBox.SelectionChanged += (sender, e) => MoveCustomCaret();
            RichTextBox.LostFocus += (sender, e) => Caret.Visibility = System.Windows.Visibility.Collapsed;
            RichTextBox.GotFocus += (sender, e) => Caret.Visibility = System.Windows.Visibility.Visible;

        }

      
        private void ReadStreamInBackgroud(object sender, DoWorkEventArgs e)
        {
            byte[] bucket = new byte[1024];
            int readLen = 0;
            // check if client is connected,  if not then connect the client
            while (tcpclient == null || !tcpclient.Connected)
            {
                tcpclient = new TcpClient();
                try
                {
                    Trace.WriteLine("Connecting.." + Session.SessionName);
                    tcpclient.Connect(Session.ServerIP, Session.ServerPort);
                    Session.IsConnected = tcpclient.Connected;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
                Thread.Sleep(500);
            }
            TelnetStream t_stream = new TelnetStream(tcpclient.GetStream());
            readLen= t_stream.Read(bucket, 0, 1024);
            this.Dispatcher.Invoke((Action)(() =>
            {
                RichTextBox.Write(Encoding.ASCII.GetString(bucket, 0, readLen));
            }));
            if (LogFileEnable)
            {
                if (logfile == null)
                    logfile = new LogFile(Session);
                logfile.Write(bucket, readLen);
            }
            if (tcpclient == null)
                return;
            // read stram asynchrounously
            Stream = tcpclient.GetStream();
            foreach (string cmd in Session.OnConnectCmdList) {
                Write(Encoding.ASCII.GetBytes(cmd));
            }

            try {
                while (true)
                {

                    if (Stream.DataAvailable)
                    {
                        readLen = Stream.Read(bucket, 0, 1024);
                        if (LogFileEnable)
                            logfile.Write(bucket, readLen);
                        if ((bucket[0] == 8 && bucket[1] == 32 && bucket[2] == 8) || (bucket[0] == 27 && bucket[1] == 91 && bucket[2] == 48 && bucket[3] == 68 && bucket[4] == 27))
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                RichTextBox.BackSpace();
                            }));
                        else
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                RichTextBox.Write(Encoding.ASCII.GetString(bucket, 0, readLen));

                            }));
                    }
                    if (_bw_Stop_Flag) // if flag is true then come out of the look, its signal for _bw to stop
                        break;
                    Thread.Sleep(100);
                }
            }catch(Exception ex)
            {
                Trace.Write(ex);
            }
        }

        public void Dispose()
        {
            //Stream.Dispose();
            logfile.Dispose();
            _bw_Stop_Flag = true;
            _bw_readstream.CancelAsync();
            if(Stream!=null)
            Stream.Dispose();
            tcpclient = null;
            
        }

        public void Connect()
        {
            if (tcpclient != null && tcpclient.Connected)
                return;
            if (_bw_readstream != null)
            {
                _bw_readstream.CancelAsync();
                _bw_Stop_Flag = true;
                Thread.Sleep(100);
            }

            _bw_readstream = new BackgroundWorker();
            _bw_readstream.WorkerSupportsCancellation = true;
            _bw_Stop_Flag = false;
            _bw_readstream.DoWork += ReadStreamInBackgroud;
            _bw_readstream.RunWorkerAsync();
        }

        public void Write(string str)
        {
            Write(Encoding.ASCII.GetBytes(str));
        }
        public void Write(byte[] b)
        {
            if (Stream == null)
                return;
            try {
                Stream.Write(b, 0, b.Length);
            }catch(Exception ex)
            {
                Trace.Write(ex);
                Connect();
            }
        }
     
        internal void MoveCustomCaret()
        {
            var caretLocation = RichTextBox.CaretPosition.GetCharacterRect(LogicalDirection.Forward);
            //var caretLocation = textbox.GetRectFromCharacterIndex(textbox.CaretIndex).Location;

            if (!double.IsInfinity(caretLocation.X))
            {
                Canvas.SetLeft(Caret, caretLocation.X);
            }

            if (!double.IsInfinity(caretLocation.Y))
            {
                Canvas.SetTop(Caret, caretLocation.Y);
            }
        }
        public void UpdateCaret()
        {
            //textbox.Select(textbox.Text.Length, 0);
            //RichTextBox.Selection.Select(run.Text.Length,);
            MoveCustomCaret();
        }
    }
}
