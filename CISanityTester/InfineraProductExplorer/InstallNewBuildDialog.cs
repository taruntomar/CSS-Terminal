using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSSTerminal.InfineraProductExplorer
{
    public enum InstallationMode
    {
        FTPServer,
        LocalDisk,
        AvalableBuild
    } 
    partial class InstallNewBuildDialog : Form
    {
        public InstallationMode InstallMode { get; set; }
        public string FTPServerIP { get; set; }
        public string FTPServerUsername { get; set; }
        public string FTPServerPassword { get; set; }
        public string FTPServerPath { get; set; }

        public string LocalDiskPath { get; set; }
        public string AvailBuildFileName { get; set; }
        private BackgroundWorker bw_worker = null;
        public InstallNewBuildDialog()
        {
            InitializeComponent();
            this.Text = "Install new build";
            RadioButtonLocalDisk.CheckedChanged += FromLocalDiskSelectionChecked;
            RadioButtonFTPServer.CheckedChanged += FTPServerSelectionChecked;
            RadioButtonAvailImg.CheckedChanged += InstallFromAvailImageSelectionChecked;

            RadioButtonLocalDisk.Checked = false;
            RadioButtonFTPServer.Checked = true;
            RadioButtonAvailImg.Checked = false;

            ListBox_AvailFileList.Enabled = false;
            TextBox_FromLocalDiskPath.Enabled = false;
            Button_BrowseLocalFilePath.Enabled = false;
            Button_Install.Enabled = false;

        }

        private void InstallFromAvailImageSelectionChecked(object sender, EventArgs e)
        {
            ListBox_AvailFileList.Enabled = RadioButtonAvailImg.Checked;
            Button_Install.Enabled = RadioButtonAvailImg.Checked;
            Button_Download.Enabled = !RadioButtonAvailImg.Checked;
            Button_DownloadandInstall.Enabled = !RadioButtonAvailImg.Checked;
        }

        private void FromLocalDiskSelectionChecked(object sender, EventArgs e)
        {
            TextBox_FromLocalDiskPath.Enabled = RadioButtonLocalDisk.Checked;
            Button_BrowseLocalFilePath.Enabled = RadioButtonLocalDisk.Checked;
        }

        private void FTPServerSelectionChecked(object sender, EventArgs e)
        {
            Label_HostIP.Enabled = RadioButtonFTPServer.Checked;
            Label_FTPUsername.Enabled = RadioButtonFTPServer.Checked;
            Label_FTPPassword.Enabled = RadioButtonFTPServer.Checked;
            Label_FTPPath.Enabled = RadioButtonFTPServer.Checked;

            TextBox_HostIP.Enabled = RadioButtonFTPServer.Checked;
            TextBox_Username.Enabled = RadioButtonFTPServer.Checked;
            TextBox_Password.Enabled = RadioButtonFTPServer.Checked;
            TextBox_FTPPath.Enabled = RadioButtonFTPServer.Checked;
            ListBox_FTPServerList.Enabled = RadioButtonFTPServer.Checked;

        }

        private void InstallNewBuildDialog_Load(object sender, EventArgs e)
        {
            
        }
      
        private void DownloadBuild(object sender, EventArgs e)
        {
            if (RadioButtonFTPServer.Checked)
            {
                // download from FTP server
                InstallMode = InstallationMode.FTPServer;
                FTPServerIP = TextBox_HostIP.Text;
                FTPServerUsername = TextBox_Username.Text;
                FTPServerPassword = TextBox_Password.Text;
                FTPServerPath = TextBox_FTPPath.Text;
               
            }else if(RadioButtonAvailImg.Checked)
            {
                InstallMode = InstallationMode.AvalableBuild;
                AvailBuildFileName = ListBox_AvailFileList.SelectedItem.ToString();
            }else
            {
                InstallMode = InstallationMode.LocalDisk;
            }
        }
    }
}
