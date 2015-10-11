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
        private bool autoreconnect = false;
        private int autoreconnecttime;
        public TerminalBox(TelnetSession session,bool LogFileEnable)
        {
            Session = session;
            autoreconnecttime = 500;
            InitializeComponent();
            RichTextBox.SetTelnetSession(session);
            RichTextBox.WriteInStream += Write;
            RichTextBox.WriteStream += WriteAsync;
            //RichTextBox.UpdateCaret += UpdateCaret;
            SetupTextBox();
            this.LogFileEnable = LogFileEnable;
            if(LogFileEnable)
            logfile = new LogFile(session);
            ShowNoConnectionBanner();
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
        private void ShowNoConnectionBanner()
        {
            TopGrid.Children.Clear();
            TextBlock textbox = new TextBlock();
            textbox.Text = "No Connection";
            textbox.FontSize = 22;
            textbox.Margin = new System.Windows.Thickness(10, 20, 0, 0);

            Button button = new Button();
            button.Content = "Reconnect";
            button.Margin = new System.Windows.Thickness(10, 20, 0, 0);
            button.Click += ReconnectButton_Click;
            button.Width = 100;

            StackPanel panel = new StackPanel();
            panel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            panel.Orientation = Orientation.Vertical;
            panel.Children.Add(textbox);
            panel.Children.Add(button);
            
            TopGrid.Children.Add(panel);
        }

        private void ReconnectButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowReconnectingPanel();
            Connect();
        }

        private void ShowReconnectingPanel()
        {

            StackPanel panel = (StackPanel)TopGrid.Children[0];
            ((TextBlock)panel.Children[0]).Text = "Connecting...";
            Button button =  ((Button)panel.Children[1]);
            button.Content = "Stop";
            button.Click -= ReconnectButton_Click;
            button.Click += StopConnecting;


            if (panel.Children.Count == 3 && panel.Children[2] is ProgressBar)
                return;

                ProgressBar pbar = new ProgressBar();
                pbar.IsIndeterminate = true;
                pbar.Margin = new System.Windows.Thickness(10, 20, 0, 0);
                pbar.Height = 10;
                pbar.Width = 350;

                panel.Children.Add(pbar);
        }

        private void StopConnecting(object sender, System.Windows.RoutedEventArgs e)
        {
            StopbackgroundThread();
            ShowNoConnectionBanner();
        }

        private void ShowTerminalRichTextBox()
        {
                TopGrid.Children.Clear();
                TopGrid.Children.Add(RichTextBox);
                TopGrid.Children.Add(caretcanvas);
        }

        private void ReadStreamInBackgroud(object sender, DoWorkEventArgs e)
        {
            byte[] bucket = new byte[1024];
            int readLen = 0;
            // check if client is connected,  if not then connect the client
            while (tcpclient == null || !tcpclient.Connected)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ShowReconnectingPanel();
                }));
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

                if (!autoreconnect)
                    break;
                else
                    Thread.Sleep(autoreconnecttime);
            }
            
            if (tcpclient == null || !tcpclient.Connected)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ShowNoConnectionBanner();
                }));
                return;
            }
            this.Dispatcher.Invoke((Action)(() =>
            {
                ShowTerminalRichTextBox();
            }));

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
            if (tcpclient == null || !tcpclient.Connected)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    ShowNoConnectionBanner();
                }));
                return;
            }
            // read stram asynchrounously
            Stream = tcpclient.GetStream();
            foreach (string cmd in Session.OnConnectCmdList) {
                Write(Encoding.ASCII.GetBytes(cmd));
            }
            int num = 0;
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
                                Trace.WriteLine("back received: "+ ++num);
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
                    if (tcpclient == null || !tcpclient.Connected)
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ShowNoConnectionBanner();
                        }));
                        return;
                    }
                }
            }catch(Exception ex)
            {
               
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        ShowNoConnectionBanner();
                    }));
                Trace.Write(ex);
            }
        }

        private void StopbackgroundThread()
        {
            _bw_Stop_Flag = true;
            _bw_readstream.CancelAsync();
            if (Stream != null)
                Stream.Dispose();
            tcpclient = null;
        }
        public void Dispose()
        {
            //Stream.Dispose();
            logfile.Dispose();
            StopbackgroundThread();

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
        public async Task<bool> WriteAsync(byte[] b)
        {
            
            byte[] bucket = new byte[1024];
            int readLen = 0;
            if (Stream == null)
                return false;
            try {
                while (!Stream.CanWrite)
                    Thread.Sleep(20);
                await Stream.WriteAsync(b, 0, b.Length);
                readLen = Stream.Read(bucket, 0, 1024);
                if (LogFileEnable)
                    logfile.Write(bucket, readLen);
                if ((bucket[0] == 8 && bucket[1] == 32 && bucket[2] == 8) || (bucket[0] == 27 && bucket[1] == 91 && bucket[2] == 48 && bucket[3] == 68 && bucket[4] == 27))
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                       // Trace.WriteLine("back received: " + ++num);
                        RichTextBox.BackSpace();
                    }));
                return true;
            }catch(Exception ex)
            {
                Trace.Write(ex);
                Connect();
                return false;
            }
        }
        public void Write(byte[] b)
        {
            if (Stream == null)
                return;
            try
            {
                Stream.Write(b, 0, b.Length);
               
            }
            catch (Exception ex)
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
