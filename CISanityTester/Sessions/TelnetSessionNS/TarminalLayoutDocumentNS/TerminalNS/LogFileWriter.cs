using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSTerminal.Sessions
{
    public class LogFile:IDisposable
    {
        public string LogFilePathWithFile { get; set; }
        public string LogDirectory { get; set; }
        private FileStream LogFileStream;
        private TelnetSession session;
        public LogFile(TelnetSession session)
        {
            this.session = session;
            LogDirectory = Template.TemplateFileHandler.GetTemplateFileHandler().TemplateAbsolutePath + @"\SessionsLogs";
            string todaylogdir = LogDirectory + "\\" + DateTime.Today.Month + "-" + DateTime.Today.Day + "-" + DateTime.Today.Year;
            LogFilePathWithFile = todaylogdir + "\\" + session.SessionName + ".txt";
            Directory.CreateDirectory(todaylogdir);
            //System.Security.Permissions.FileIOPermissionAccess.
            // File.Open(todaylogdir + "\\" + "Hello.txt", FileMode.Create);
            LogFileStream = File.Open(LogFilePathWithFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
        }

        internal void Write(byte[] bucket, int readLen)
        {
            if(LogFileStream!=null)
           LogFileStream.WriteAsync(bucket, 0, readLen);
        }

        public void Dispose()
        {

            LogFilePathWithFile = null;
            LogDirectory = null;

            LogFileStream.Close();
            LogFileStream.Dispose();
            LogFileStream = null;

        }
    }
}
