
using CISanityTester.Sessions.TelnetSessionNS.TermianlLayoutDocumentNS;
using System;
using System.Collections.Generic;

namespace CISanityTester.Sessions
{
    public class TelnetSession:IDisposable
    {
        internal string FontSize;

        private TermianlLayoutDocument TerminalLayoutDocumentObj { get; set; }

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
            SessionName = null;
            ServerIP = null;
            TerminalLayoutDocumentObj.Close();
            TerminalLayoutDocumentObj.Dispose();
            
        }

        public TelnetSession(string ip, int port, string name,string sessionprotocol, bool OpenTerminalLayoutDocument,bool startoncreate)
        {
            ServerIP = ip;
            ServerPort = port;
            SessionName = name;
            SessionType = sessionprotocol;
            OnConnectCmdList = new List<string>();
            TerminalLayoutDocumentObj = new TermianlLayoutDocument(this,OpenTerminalLayoutDocument);

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

