using System;
using System.Windows.Forms;

namespace CISanityTester.Sessions
{
    partial class SessionProperty : Form
    {
        public TelnetSession telnetsession { get; set; }

        private bool newsession = false;

        public SessionProperty()
        {
            // constructor for new telnet Session
            InitializeComponent();
            this.Text = "New session";
            newsession = true;
            comboBox1.SelectedIndex = 0;
        }
        public SessionProperty(TelnetSession session)
        {
            telnetsession = session;
            InitializeComponent();
            this.Text = session.SessionName+ " - Session Property";

            // fill session name and connection parameter
            TextBox_SessionName.Text = telnetsession.SessionName;
            TextBox_ServerIP.Text = telnetsession.ServerIP;
            TextBox_PortNo.Text = Convert.ToString(telnetsession.ServerPort);
            //TextBox_BufferCount.Text = telnetsession.consolewindow.

            //TextBox_FontName.Text = telnetsession.FontFamily;
            //TextBox_FontSize.Text = telnetsession.FontSize;
            
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (newsession)
            {
                telnetsession = new TelnetSession(TextBox_ServerIP.Text, Convert.ToInt32(TextBox_PortNo.Text), TextBox_SessionName.Text,comboBox1.SelectedItem.ToString(), true,true);
                telnetsession.OnConnectCmdList.Add(textBox11.Text);
                telnetsession.OnConnectCmdList.Add(textBox12.Text);

            }
            else
            {
                //telnetsession.consolewindow.ConsoleTextBox.FontSize = Convert.ToDouble(TextBox_FontSize.Text);
                if (TextBox_ServerIP.Text != telnetsession.ServerIP || TextBox_PortNo.Text != telnetsession.ServerPort.ToString())
                {
                    telnetsession.SessionName = TextBox_ServerIP.Text;
                    telnetsession.ServerPort = Convert.ToInt32(TextBox_PortNo.Text);
                    telnetsession.Connect();
                    //telnetsession.StartReadingStreamThread();
                }

                telnetsession.OnConnectCmdList.Clear();
                telnetsession.OnConnectCmdList.Add(textBox11.Text);
                telnetsession.OnConnectCmdList.Add(textBox12.Text);
            }
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
