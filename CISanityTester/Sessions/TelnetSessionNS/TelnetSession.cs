
using CSSTerminal.Sessions.TelnetSessionNS.TermianlLayoutDocumentNS;
using System;
using System.Collections.Generic;

namespace CSSTerminal.Sessions
{
    public class TelnetSession:IDisposable
    {
        internal string FontSize;

        private TermianlLayoutDocument TerminalLayoutDocumentObj { get; set; }
        private bool LogFileEnable= false;
        public string SessionName { get; set; }
        public string ServerIP { get; set; }
        public string SessionType { get; set; }
        public int ServerPort { get; set; }
        public bool? IsConnected { get; set; }
        public object CommandOutput { get; internal set; }
        public object FontFamily { get; internal set; }
        public List<string> OnConnectCmdList;

        public void Dispose()
        {
            if (TerminalLayoutDocumentObj != null )
            {
                if(TerminalLayoutDocumentObj.IsOpen)
                TerminalLayoutDocumentObj.Close();
                TerminalLayoutDocumentObj.Dispose();
            }
            

            SessionName = null;
            ServerIP = null;

        }

        public TelnetSession(string ip, int port, string name,string sessionprotocol, bool OpenTerminalLayoutDocument,bool startoncreate,bool LogFileEnable)
        {
            ServerIP = ip;
            ServerPort = port;
            SessionName = name;
            SessionType = sessionprotocol;
            this.LogFileEnable = LogFileEnable;
            OnConnectCmdList = new List<string>();
            TerminalLayoutDocumentObj = new TermianlLayoutDocument(this,OpenTerminalLayoutDocument, LogFileEnable);

            if(startoncreate)
            Connect();
        }

        internal void Open()
        {
            TerminalLayoutDocumentObj.Open();
        }

        public void Connect()
        {
            TerminalLayoutDocumentObj.Connect();
        }

        public void Write(String str)
        {
            TerminalLayoutDocumentObj.Write(str);
        }

        
        

    }
}

