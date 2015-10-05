namespace CSSTerminal.InfineraProductExplorer
{
    partial class InstallNewBuildDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Button_Download = new System.Windows.Forms.Button();
            this.Button_DownloadandInstall = new System.Windows.Forms.Button();
            this.RadioButtonLocalDisk = new System.Windows.Forms.RadioButton();
            this.TextBox_FromLocalDiskPath = new System.Windows.Forms.TextBox();
            this.Button_BrowseLocalFilePath = new System.Windows.Forms.Button();
            this.RadioButtonFTPServer = new System.Windows.Forms.RadioButton();
            this.TextBox_HostIP = new System.Windows.Forms.TextBox();
            this.Label_HostIP = new System.Windows.Forms.Label();
            this.Label_FTPUsername = new System.Windows.Forms.Label();
            this.TextBox_Username = new System.Windows.Forms.TextBox();
            this.Label_FTPPassword = new System.Windows.Forms.Label();
            this.TextBox_Password = new System.Windows.Forms.TextBox();
            this.Label_FTPPath = new System.Windows.Forms.Label();
            this.TextBox_FTPPath = new System.Windows.Forms.TextBox();
            this.RadioButtonAvailImg = new System.Windows.Forms.RadioButton();
            this.ListBox_AvailFileList = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.Button_Install = new System.Windows.Forms.Button();
            this.ListBox_FTPServerList = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.Button_Download.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Download.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Button_Download.Location = new System.Drawing.Point(149, 416);
            this.Button_Download.Name = "okButton";
            this.Button_Download.Size = new System.Drawing.Size(75, 23);
            this.Button_Download.TabIndex = 24;
            this.Button_Download.Text = "&Download";
            this.Button_Download.Click += new System.EventHandler(this.DownloadBuild);
            // 
            // button1
            // 
            this.Button_DownloadandInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_DownloadandInstall.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Button_DownloadandInstall.Location = new System.Drawing.Point(230, 416);
            this.Button_DownloadandInstall.Name = "button1";
            this.Button_DownloadandInstall.Size = new System.Drawing.Size(122, 23);
            this.Button_DownloadandInstall.TabIndex = 26;
            this.Button_DownloadandInstall.Text = "&Download and Install";
            // 
            // radioButton1
            // 
            this.RadioButtonLocalDisk.AutoSize = true;
            this.RadioButtonLocalDisk.Location = new System.Drawing.Point(14, 32);
            this.RadioButtonLocalDisk.Name = "radioButton1";
            this.RadioButtonLocalDisk.Size = new System.Drawing.Size(100, 17);
            this.RadioButtonLocalDisk.TabIndex = 27;
            this.RadioButtonLocalDisk.TabStop = true;
            this.RadioButtonLocalDisk.Text = "Install from local";
            this.RadioButtonLocalDisk.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.TextBox_FromLocalDiskPath.Location = new System.Drawing.Point(14, 61);
            this.TextBox_FromLocalDiskPath.Name = "textBox1";
            this.TextBox_FromLocalDiskPath.Size = new System.Drawing.Size(316, 20);
            this.TextBox_FromLocalDiskPath.TabIndex = 28;
            // 
            // button2
            // 
            this.Button_BrowseLocalFilePath.Location = new System.Drawing.Point(336, 58);
            this.Button_BrowseLocalFilePath.Name = "button2";
            this.Button_BrowseLocalFilePath.Size = new System.Drawing.Size(75, 23);
            this.Button_BrowseLocalFilePath.TabIndex = 29;
            this.Button_BrowseLocalFilePath.Text = "Browse";
            this.Button_BrowseLocalFilePath.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.RadioButtonFTPServer.AutoSize = true;
            this.RadioButtonFTPServer.Location = new System.Drawing.Point(17, 104);
            this.RadioButtonFTPServer.Name = "radioButton2";
            this.RadioButtonFTPServer.Size = new System.Drawing.Size(135, 17);
            this.RadioButtonFTPServer.TabIndex = 30;
            this.RadioButtonFTPServer.TabStop = true;
            this.RadioButtonFTPServer.Text = "From other FTP Server:";
            this.RadioButtonFTPServer.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.TextBox_HostIP.Location = new System.Drawing.Point(78, 141);
            this.TextBox_HostIP.Name = "textBox2";
            this.TextBox_HostIP.Size = new System.Drawing.Size(169, 20);
            this.TextBox_HostIP.TabIndex = 31;
            // 
            // label1
            // 
            this.Label_HostIP.AutoSize = true;
            this.Label_HostIP.Location = new System.Drawing.Point(14, 144);
            this.Label_HostIP.Name = "label1";
            this.Label_HostIP.Size = new System.Drawing.Size(32, 13);
            this.Label_HostIP.TabIndex = 32;
            this.Label_HostIP.Text = "Host:";
            // 
            // label2
            // 
            this.Label_FTPUsername.AutoSize = true;
            this.Label_FTPUsername.Location = new System.Drawing.Point(12, 170);
            this.Label_FTPUsername.Name = "label2";
            this.Label_FTPUsername.Size = new System.Drawing.Size(58, 13);
            this.Label_FTPUsername.TabIndex = 34;
            this.Label_FTPUsername.Text = "Username:";
            // 
            // textBox3
            // 
            this.TextBox_Username.Location = new System.Drawing.Point(78, 167);
            this.TextBox_Username.Name = "textBox3";
            this.TextBox_Username.Size = new System.Drawing.Size(169, 20);
            this.TextBox_Username.TabIndex = 33;
            // 
            // label3
            // 
            this.Label_FTPPassword.AutoSize = true;
            this.Label_FTPPassword.Location = new System.Drawing.Point(12, 196);
            this.Label_FTPPassword.Name = "label3";
            this.Label_FTPPassword.Size = new System.Drawing.Size(56, 13);
            this.Label_FTPPassword.TabIndex = 36;
            this.Label_FTPPassword.Text = "Password:";
            // 
            // textBox4
            // 
            this.TextBox_Password.Location = new System.Drawing.Point(78, 193);
            this.TextBox_Password.Name = "textBox4";
            this.TextBox_Password.Size = new System.Drawing.Size(169, 20);
            this.TextBox_Password.TabIndex = 35;
            // 
            // label4
            // 
            this.Label_FTPPath.AutoSize = true;
            this.Label_FTPPath.Location = new System.Drawing.Point(12, 222);
            this.Label_FTPPath.Name = "label4";
            this.Label_FTPPath.Size = new System.Drawing.Size(32, 13);
            this.Label_FTPPath.TabIndex = 38;
            this.Label_FTPPath.Text = "Path:";
            // 
            // textBox5
            // 
            this.TextBox_FTPPath.Location = new System.Drawing.Point(78, 219);
            this.TextBox_FTPPath.Name = "textBox5";
            this.TextBox_FTPPath.Size = new System.Drawing.Size(169, 20);
            this.TextBox_FTPPath.TabIndex = 37;
            // 
            // radioButton3
            // 
            this.RadioButtonAvailImg.AutoSize = true;
            this.RadioButtonAvailImg.Location = new System.Drawing.Point(15, 273);
            this.RadioButtonAvailImg.Name = "radioButton3";
            this.RadioButtonAvailImg.Size = new System.Drawing.Size(145, 17);
            this.RadioButtonAvailImg.TabIndex = 39;
            this.RadioButtonAvailImg.TabStop = true;
            this.RadioButtonAvailImg.Text = "From available file on NE:";
            this.RadioButtonAvailImg.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.ListBox_AvailFileList.FormattingEnabled = true;
            this.ListBox_AvailFileList.Location = new System.Drawing.Point(15, 305);
            this.ListBox_AvailFileList.Name = "listBox1";
            this.ListBox_AvailFileList.Size = new System.Drawing.Size(396, 69);
            this.ListBox_AvailFileList.TabIndex = 40;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.Location = new System.Drawing.Point(358, 416);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 41;
            this.button3.Text = "&Cancel";
            // 
            // button4
            // 
            this.Button_Install.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Install.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Button_Install.Location = new System.Drawing.Point(68, 416);
            this.Button_Install.Name = "button4";
            this.Button_Install.Size = new System.Drawing.Size(75, 23);
            this.Button_Install.TabIndex = 42;
            this.Button_Install.Text = "&Install";
            // 
            // listView1
            // 
            this.ListBox_FTPServerList.Location = new System.Drawing.Point(273, 142);
            this.ListBox_FTPServerList.Name = "listView1";
            this.ListBox_FTPServerList.Size = new System.Drawing.Size(138, 96);
            this.ListBox_FTPServerList.TabIndex = 43;
            this.ListBox_FTPServerList.UseCompatibleStateImageBehavior = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ListBox_FTPServerList);
            this.groupBox1.Controls.Add(this.ListBox_AvailFileList);
            this.groupBox1.Controls.Add(this.RadioButtonAvailImg);
            this.groupBox1.Controls.Add(this.Label_FTPPath);
            this.groupBox1.Controls.Add(this.TextBox_FTPPath);
            this.groupBox1.Controls.Add(this.Label_FTPPassword);
            this.groupBox1.Controls.Add(this.TextBox_Password);
            this.groupBox1.Controls.Add(this.Label_FTPUsername);
            this.groupBox1.Controls.Add(this.TextBox_Username);
            this.groupBox1.Controls.Add(this.Label_HostIP);
            this.groupBox1.Controls.Add(this.TextBox_HostIP);
            this.groupBox1.Controls.Add(this.RadioButtonFTPServer);
            this.groupBox1.Controls.Add(this.Button_BrowseLocalFilePath);
            this.groupBox1.Controls.Add(this.TextBox_FromLocalDiskPath);
            this.groupBox1.Controls.Add(this.RadioButtonLocalDisk);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(425, 393);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Install Build";
            // 
            // InstallNewBuildDialog
            // 
            this.AcceptButton = this.Button_Download;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 451);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Button_Install);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.Button_DownloadandInstall);
            this.Controls.Add(this.Button_Download);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstallNewBuildDialog";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InstallNewBuildDialog";
            this.Load += new System.EventHandler(this.InstallNewBuildDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Button_Download;
        private System.Windows.Forms.Button Button_DownloadandInstall;
        private System.Windows.Forms.RadioButton RadioButtonLocalDisk;
        private System.Windows.Forms.TextBox TextBox_FromLocalDiskPath;
        private System.Windows.Forms.Button Button_BrowseLocalFilePath;
        private System.Windows.Forms.RadioButton RadioButtonFTPServer;
        private System.Windows.Forms.TextBox TextBox_HostIP;
        private System.Windows.Forms.Label Label_HostIP;
        private System.Windows.Forms.Label Label_FTPUsername;
        private System.Windows.Forms.TextBox TextBox_Username;
        private System.Windows.Forms.Label Label_FTPPassword;
        private System.Windows.Forms.TextBox TextBox_Password;
        private System.Windows.Forms.Label Label_FTPPath;
        private System.Windows.Forms.TextBox TextBox_FTPPath;
        private System.Windows.Forms.RadioButton RadioButtonAvailImg;
        private System.Windows.Forms.ListBox ListBox_AvailFileList;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button Button_Install;
        private System.Windows.Forms.ListView ListBox_FTPServerList;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
