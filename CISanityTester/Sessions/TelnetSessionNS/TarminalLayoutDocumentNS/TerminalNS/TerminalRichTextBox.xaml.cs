using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace CISanityTester.Sessions.TelnetSessionNS.TarminalLayoutDocumentNS.TerminalNS
{
    /// <summary>
    /// Interaction logic for TerminalRichTextBox.xaml
    /// </summary>
    public partial class TerminalRichTextBox : RichTextBox
    {
        public event Action<byte[]> WriteInStream;
        //public event Action UpdateCaret;
        public int endposition=0;
        public int capacity = 10000;
        private List<String> HistCmdList;
        private StringBuilder currentcmd;

        private int HistCmdIndex=0;
        private TelnetSession session;
        private string CommandEndByte;
        private Run HistRun;
        public TerminalRichTextBox()
        {
           
            InitializeComponent();
            SetContextMenu();
            HistCmdList = new List<string>();
            HistCmdList.Add("");
            currentcmd = new StringBuilder();
            HistRun = new Run();

        }
        public void SetTelnetSession(TelnetSession session)
        {
            this.session = session;
            if (session.SessionType == "TL1")
                CommandEndByte = ";";
            else
                CommandEndByte = "\r";
        }

        private void WriteInCommandHist(byte[] obj)
        {
            if (obj[0] == 0x08)
                currentcmd.Remove(currentcmd.Length - 1, 1);
            else
                currentcmd.Append(Encoding.ASCII.GetString(obj));
        }

        private void SetContextMenu()
        {
            ContextMenu = new ContextMenu();

            MenuItem itm = new MenuItem() { Header = "Copy, Ctrl+Insert" };
            itm.Click += new RoutedEventHandler(CopyButtonClick);
            // = Keys.Control | Keys.C;
            ContextMenu.Items.Add(itm);

            itm = new MenuItem() { Header = "Paste, Shift+Insert" };
            itm.Click += new RoutedEventHandler(PasteButtonClick);
            ContextMenu.Items.Add(itm);
        }

        private void CopyButtonClick(object sender, RoutedEventArgs args)
        {
             Clipboard.SetText(this.Selection.Text);
        }

        private void PasteButtonClick(object sender, RoutedEventArgs args)
        {
            WriteInStream(Encoding.ASCII.GetBytes(Clipboard.GetText()));
        }

        public void Write(string str)
        {
            if (capacity < str.Length)
            {
                // implement limited buffer concept
                int require = str.Length - capacity;
                List<Run> tmprun = new List<Run>();
                foreach(Run run in ParaText.Inlines)
                {
                    if (run.Text.Length <= require)
                    {
                        tmprun.Add(run);
                        require = require - run.Text.Length;
                        if (require == 0)
                            break;
                    }
                    else
                    {
                        run.Text = run.Text.Remove(0, require);
                        break;
                    }
                    
                }
                
                foreach(Run run in tmprun)
                    ParaText.Inlines.Remove(run);

                capacity = capacity + str.Length;
            }
            ParaText.Inlines.Add(new Run(str));
            capacity = capacity - str.Length;
            endposition += str.Length;
            MoveCursorToEnd();
            ScrollToEnd();
        }

        public void MoveCursorToEnd()
        {
            Selection.Select(Document.ContentEnd, Document.ContentEnd);
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            //base.OnTextInput(e);
            if(ParaText.Inlines.LastInline == HistRun)
            {
                ParaText.Inlines.Remove(HistRun);
                WriteInStream(Encoding.ASCII.GetBytes(HistRun.Text));
            }

            WriteInStream(Encoding.ASCII.GetBytes(e.Text));
            
            if(e.Text == CommandEndByte)
            {
                if (currentcmd.Length == 0 || currentcmd.ToString().Trim().Length == 0)
                    return;
                string tmp = currentcmd.ToString();
                if(!HistCmdList.Contains(tmp))
                HistCmdList.Add(tmp);
                HistCmdIndex = (HistCmdList.Count-2);
                currentcmd.Clear();
            }else
            {
                currentcmd.Append(e.Text);
            }
        }

        private void RTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Back)
            {
                if (ParaText.Inlines.LastInline == HistRun)
                {
                    ParaText.Inlines.Remove(HistRun);
                    currentcmd.Remove(currentcmd.Length - 1, 1);
                    WriteInStream(Encoding.ASCII.GetBytes(currentcmd.ToString()));
                }
                else
                {
                    WriteInStream(new byte[] { 0x08 });
                    if (currentcmd.Length > 0)
                        currentcmd.Remove(currentcmd.Length - 1, 1);
                }
                
            }
            else if (e.Key == Key.Space)
            {
                if (ParaText.Inlines.LastInline == HistRun)
                {
                    ParaText.Inlines.Remove(HistRun);
                    WriteInStream(Encoding.ASCII.GetBytes(HistRun.Text));
                }
                WriteInStream(new byte[] { 32 });
                currentcmd.Append(" ");
            }


            else if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                WriteInStream(new byte[] { 3 });
                //MessageBox.Show("CTRL + C was pressed");
            }
            else if (e.Key == Key.Insert && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                CopyButtonClick(null, null);
            else if (e.Key == Key.Insert && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                PasteButtonClick(null, null);
            else if (e.Key == Key.Up && HistCmdList.Count != 0)
            {
                HistCmdIndex++;
                if (HistCmdIndex == HistCmdList.Count)
                    HistCmdIndex =0;
                    
                HistRun.Text = HistCmdList[HistCmdIndex];
                currentcmd.Clear();
                currentcmd.Append(HistRun.Text);
                if (ParaText.Inlines.LastInline!=HistRun)
                ParaText.Inlines.Add(HistRun);
                MoveCursorToEnd();
            }

            else if (e.Key == Key.Down && HistCmdList.Count != 0)
            {
                HistCmdIndex--;
                if (HistCmdIndex <0)
                    HistCmdIndex = (HistCmdList.Count-1);
                HistRun.Text = HistCmdList[HistCmdIndex];
                currentcmd.Clear();
                currentcmd.Append(HistRun.Text);
                if (ParaText.Inlines.LastInline != HistRun)
                    ParaText.Inlines.Add(HistRun);
                MoveCursorToEnd();
            }
            

        }

        public void BackSpace()
        {
            Run run = ((Run)ParaText.Inlines.LastInline);
            run.Text = run.Text.Remove(run.Text.Length - 1);
            endposition -= 1;
            MoveCursorToEnd();
            if (run.Text.Length == 0)
                ParaText.Inlines.Remove(run);
            return;
        }
    }

    public enum ASCIIKeyValues
    {
        Backspace = 0x08
        
    }
}
