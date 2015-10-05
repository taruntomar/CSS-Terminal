using CSSTerminal.GUIBase;
using CSSTerminal.Template.Entities.InfineraProductLine;
using System.Windows.Controls;
using System.Windows;
using System.Threading;
using System.ComponentModel;
using CSSTerminal.Template;

namespace CSSTerminal.InfineraProductExplorer
{
    class NE_Control:EquipmentBaseController
    {

        public NE_Control(IPExplorer exp) : base(exp) { }
        
        
        public void LoadNEs()
        {
            template = TemplateFileHandler.GetTemplateFileHandler().CurrentTemplate;
            if (template.NEs == null)
                return;
            foreach (NetworkElement NE in template.NEs)
            {
                HierarchicalObjectViewModel node = ipexplorer.AddTreeItem(NE.Name);
                ContextMenu cm = new ContextMenu();

                MenuItem itm = new MenuItem() { Header = "Login" };
                itm.Click += new RoutedEventHandler(Login);
                cm.Items.Add(itm);

                itm = new MenuItem() { Header = "Install New Build" };
                itm.Click += new RoutedEventHandler(InstallNewBuild);
                cm.Items.Add(itm);

                itm = new MenuItem() { Header = "Download DebugXFR" };
                itm.Click += new RoutedEventHandler(DownloadDebugXFR);
                cm.Items.Add(itm);

                itm = new MenuItem() { Header = "Delete Backup Db and SW" };
                itm.Click += new RoutedEventHandler(DeleteOldDBSW);
                cm.Items.Add(itm);

                itm = new MenuItem() { Header = "Delete Backup SW" };
                itm.Click += new RoutedEventHandler(DeleteBackupSW);
                cm.Items.Add(itm);

                itm = new MenuItem() { Header = "Delete Backup DB" };
                itm.Click += new RoutedEventHandler(DeleteBackupDB);
                cm.Items.Add(itm);

                itm = new MenuItem() { Header = "Reset Password" };
                itm.Click += new RoutedEventHandler(ResetPassword);
                cm.Items.Add(itm);

                itm = new MenuItem() { Header = "Change Permission" };
                itm.Click += new RoutedEventHandler(ChangePermission);
                cm.Items.Add(itm);

                itm = new MenuItem() { Header = "Retrive Equipments" };
                itm.Click += new RoutedEventHandler(RtrvEqpt);
                cm.Items.Add(itm);

                node.ContextMenuObj = cm;
            }
        }

        private void ChangePermission(object sender, RoutedEventArgs e)
        {
            NE_TL1.Write("ED-USER-SECU::secadmin:c11::,,,MA&SA&TT&NE&NA&PR:TMOUT=0,PWDCHNG=FALSE;");
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            NE_TL1.Write("act-user::secadmin:c::Infinera1;");

        }
        private void RtrvEqpt(object sender, RoutedEventArgs e)
        {
            NE_TL1.Write("rtrv-eqpt:::c;");

        }
        private void ResetPassword(object sender, RoutedEventArgs e)
        {
            
            NE_TL1.Write("ed-pid::secadmin:c::Infinera1,Infinera2;");
            NE_TL1.Write("ed-pid::secadmin:c::Infinera2,Infinera3;");
            NE_TL1.Write("ed-pid::secadmin:c::Infinera3,Infinera4;");
            NE_TL1.Write("ed-pid::secadmin:c::Infinera4,Infinera5;");
            NE_TL1.Write("ed-pid::secadmin:c::Infinera5,Infinera6;");
            NE_TL1.Write("ed-pid::secadmin:c::Infinera6,Infinera7;");
            NE_TL1.Write("ed-pid::secadmin:c::Infinera7,Infinera1;");

        }

        private void DeleteBackupDB(object sender, RoutedEventArgs e)
        {
            bw_worker = new BackgroundWorker();
            bw_worker.DoWork += DELETE_BACKUP_DB;
            bw_worker.RunWorkerAsync();    
        }

        private void DELETE_BACKUP_DB(object sender, DoWorkEventArgs e)
        {
                     
            NE_TL1.Write("rtrv-rfile:::c:::TYPE=DB;");
            Thread.Sleep(1200);
            string output = NE_TL1.CommandOutput.ToString();
            if (output.Contains("PRIVILEGE, LOGIN NOT ACTIVE"))
            {
                Login(null,null);
                NE_TL1.Write("rtrv-rfile:::c:::TYPE=DB;");
                Thread.Sleep(1200);
            }
            output = NE_TL1.CommandOutput.ToString();
            string[] lines = output.Split(new char[] { '\r', '\n' });

            foreach (string line in lines)
            {
                if (line.Contains("USAGE=Backup"))
                {
                    string dbname = line.Substring(line.IndexOf("FILE=") + 5, 25);
                    string cmd = "dlt-rfile:::c:::FILE=\"" + "file:///" + dbname + "\",TYPE=DB;";
                    NE_TL1.Write(cmd);
                }
            }
        }

        private void DeleteBackupSW(object sender, RoutedEventArgs e)
        {
            bw_worker = new BackgroundWorker();
            bw_worker.DoWork += DELET_BACKUP_SW;
            bw_worker.RunWorkerAsync();

        }
        private void DELET_BACKUP_SW(object sender, DoWorkEventArgs e)
        {
            NE_TL1.Write("rtrv-rfile:::c:::TYPE=SW;");
            string output = NE_TL1.CommandOutput.ToString();
            if (output.Contains("PRIVILEGE, LOGIN NOT ACTIVE"))
            {
                Login(null,null);
                NE_TL1.Write("rtrv-rfile:::c:::TYPE=SW;");
            }
            output = NE_TL1.CommandOutput.ToString();
            string[] lines = output.Split(new char[] { '\r', '\n' });

            foreach (string line in lines)
            {
                if (line.Contains("USAGE=Current"))
                {
                    string dbname = line.Substring(line.IndexOf("FILE=") + 5, 25);
                    string cmd = "dlt-rfile:::c:::FILE=\"" + "file:///" + dbname + "\",TYPE=SW;";
                    NE_TL1.Write(cmd);
                }
            }
        }
        private void DeleteOldDBSW(object sender, RoutedEventArgs e)
        {
            DeleteBackupSW(sender,e);
            DeleteBackupDB(sender,e);
        }
        private void DownloadDebugXFR(object sender, RoutedEventArgs e)
        {
            NE_TL1.Write("rtrv-rfile:::c:::TYPE=DB;");
        }

        private void InstallNewBuild(object sender, RoutedEventArgs e)
        {
            InstallNewBuildDialog dlg = new InstallNewBuildDialog();
            dlg.ShowDialog();
            bw_worker = new BackgroundWorker();
            bw_worker.DoWork += INSTALL_NEWBUILD;
            if (dlg.InstallMode == InstallationMode.FTPServer)
            {
                string cmd1 = "copy-rfile:::c:::type = swdl,src =\"ftp://"+dlg.FTPServerUsername+":"+dlg.FTPServerPassword+"@"+dlg.FTPServerIP+"///"+dlg.FTPServerPath+"\";";
                string cmd2 = "apply:::k:::filename=\"file:///M15.4.0.8071\",opertype=install;";
                bw_worker.RunWorkerAsync(new string[] { cmd1, cmd2 });
            }
        }
        private void INSTALL_NEWBUILD(object sender, DoWorkEventArgs e)
        {
            string[] cmds = (string[])e.Argument;
            NE_TL1.Write(cmds[0]);
            // code to wait for download to be completed
            NE_TL1.Write(cmds[1]);
        }

    }
}
