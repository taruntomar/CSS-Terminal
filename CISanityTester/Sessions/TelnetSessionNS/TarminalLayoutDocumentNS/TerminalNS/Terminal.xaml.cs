using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.AvalonDock.Layout;

namespace CISanityTester.Sessions
{
    /// <summary>
    /// Interaction logic for SessionConsole.xaml
    /// </summary>
    public partial class Terminal : UserControl
    {
     
        private TelnetSession Session;
        public BufferString consolebuffer { get; set; }
        public bool IsActive { get; set; }
        public LogFile logfile { get; set; }
        public delegate void OnTextArrived(byte[] buffer,int len);
        public event OnTextArrived TextArrived;
        private bool HighBufferMode = false;


        //public TerminalRichTextEditor TerminalRichTextEditorObj { get; set; }


        public Terminal(TelnetSession Session) : base()
        {
            // it should not be less than 10000
           
            this.Session = Session;
            InitializeComponent();
            ConsoleTextBox.Caret = this.Caret;
            consolebuffer = new BufferString();
            logfile = new LogFile(this);
            ConsoleTextBox.TextInput += ConsoleTextBox_TextInput;
            //ConsoleTextBox.KeyDownEvent += ConsoleTextBox_KeyDown;
            ConsoleTextBox.AddHandler(KeyDownEvent, new RoutedEventHandler(ConsoleTextBox_KeyDown), true);
            ConsoleTextBox.AddHandler(KeyUpEvent, new RoutedEventHandler(ConsoleTextBox_KeyUp), true);
        }

      
        
        private void StartReadFromStream(object sender, DoWorkEventArgs e)
        {
            ConnectSession();
            int bufferlenght = 1024;
            byte[] bucket = new byte[bufferlenght];
            int readLen = 0;
            HighBufferMode = false;

            // check how meany bytes stream has
            while (true)
            {
                try
                {
                    readLen = Stream.Read(bucket, 0, bufferlenght);
                }
                catch { }
                if (readLen == 0)
                    continue;
                logfile.Write(bucket, readLen);

                if (readLen < bufferlenght && HighBufferMode)
                    HighBufferMode = false;
                if (readLen == bufferlenght && !HighBufferMode)
                    HighBufferMode = true;

                if (HighBufferMode)
                    Thread.Sleep(20);


                if (LayoutDocument != null & LayoutDocument.IsActive)
                    StreamWorkerBW.ReportProgress(readLen, bucket);
                else
                    LayoutDocument.consolebuffer.Append(bucket, readLen);

            }

        }
        internal void SendCommand(string command)
        {
            CommandOutput.Clear();
            foreach (char c in command)
                WriteToStream(c);
        }



        public void WriteToStream(char str)
        {
            if (Stream == null)
                return;
            byte[] ba = Encoding.ASCII.GetBytes(new char[] { str });
            Stream.Write(ba, 0, ba.Length);
        }


        private Key? pressedkey1=null;
        private Key? pressedkey2 = null;
        private void ConsoleTextBox_KeyDown(object sender,RoutedEventArgs e)
        {
            KeyEventArgs ke = (KeyEventArgs)e;
            
            if (ke.Key == System.Windows.Input.Key.Enter)
            {
                if (Session.IsConnected.HasValue && Session.IsConnected.Value == false)
                {
                    Session.StreamWorkerBW.CancelAsync();

                    if(!Session.StreamWorkerBW.IsBusy)
                    Session.StreamWorkerBW.RunWorkerAsync();
                    return;
                }
            }
            else if (ke.Key == System.Windows.Input.Key.Space)
                Session.WriteToStream(' ');

            else if (ke.Key == System.Windows.Input.Key.Back)
            {
                // { 0x08, 0x20, 0x08 });
                Session.Stream.Write(new byte[] { 0x08 }, 0, 1);


            }
            else{
                if (ke.Key == Key.LeftCtrl || ke.Key ==Key.LeftShift || ke.Key==Key.RightShift)
                    pressedkey1 = ke.Key;
                else
                {
                    if(pressedkey1 == Key.LeftCtrl)
                    {

                        if (ke.Key == Key.C)
                        {
                            Session.Stream.Write(new byte[] { 3 }, 0, 1);
                        }else if(ke.Key == Key.Insert)
                        {
                            if(ConsoleTextBox.SelectedText!="")
                            System.Windows.Forms.Clipboard.SetText(ConsoleTextBox.SelectedText);
                        }

                    }
                    else
                    {
                        if(( pressedkey1 == Key.LeftShift || pressedkey1 == Key.RightShift ) && (ke.Key==Key.Insert))
                        {
                            // code for paste
                            string tmp = System.Windows.Forms.Clipboard.GetText();
                            foreach(char c in tmp)
                            Session.WriteToStream(c);
                        }
                    }
                }
            }
        }
        private void ConsoleTextBox_KeyUp(object sender, RoutedEventArgs e)
        {
            KeyEventArgs ke = (KeyEventArgs)e;
            if (ke.Key == pressedkey1)
                pressedkey1 = null;
            if (ke.Key == pressedkey2)
                pressedkey2 = null;

        }

        private void ConsoleTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "")
                return;
            Session.WriteToStream(Convert.ToChar(e.Text));
        }



        internal void Write(byte[] bucket, int length)
        {
            if (TextArrived != null)
                   TextArrived(bucket,length);

          
        }

       
        public LayoutContent LayoutDocumnet
        {
            get {
                if (layoutdocument == null)
                {
                    layoutdocument = new TermianlLayoutDocument();
                    layoutdocument.Title = Session.SessionName;
                    layoutdocument.Content = this;
                    layoutdocument.Closed += LayoutDocumentClose;
                    layoutdocument.ActiveChanged += Layoutdocument_ActiveChanged;
                    layoutdocument.ToolTip = Session.ServerIP + ":" + Session.ServerPort;
                    Layoutdocument_ActiveChanged(false);
                }

                return layoutdocument;
            }
        }

        private void Layoutdocument_ActiveChanged(bool value)
        {
            if (value)
            {
                ConsoleTextBox.Write(consolebuffer.Text);
                consolebuffer.Clear();
                ConsoleTextBox.SetFocus();
            }
            IsActive = value;
        }

        private void LayoutDocumentClose(object sender, EventArgs e)
        {
            Session.IsLayoutdocumentClosed = true;
            //Session.Dispose();
        }
    }
}

