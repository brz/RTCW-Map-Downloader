namespace RTCWMapDownloader
{
    partial class frmDownload
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.gpbDownload = new System.Windows.Forms.GroupBox();
            this.pbMain = new System.Windows.Forms.ProgressBar();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblFilesize = new System.Windows.Forms.Label();
            this.lblDownloaded = new System.Windows.Forms.Label();
            this.lblFile = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pbLevelshot = new System.Windows.Forms.PictureBox();
            this.gpbDownload.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLevelshot)).BeginInit();
            this.SuspendLayout();
            // 
            // gpbDownload
            // 
            this.gpbDownload.Controls.Add(this.pbMain);
            this.gpbDownload.Controls.Add(this.lblSpeed);
            this.gpbDownload.Controls.Add(this.lblFilesize);
            this.gpbDownload.Controls.Add(this.lblDownloaded);
            this.gpbDownload.Controls.Add(this.lblFile);
            this.gpbDownload.Location = new System.Drawing.Point(218, 12);
            this.gpbDownload.Name = "gpbDownload";
            this.gpbDownload.Size = new System.Drawing.Size(320, 165);
            this.gpbDownload.TabIndex = 0;
            this.gpbDownload.TabStop = false;
            this.gpbDownload.Text = "Download";
            // 
            // pbMain
            // 
            this.pbMain.Location = new System.Drawing.Point(6, 136);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(308, 23);
            this.pbMain.TabIndex = 4;
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(35, 105);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(41, 13);
            this.lblSpeed.TabIndex = 3;
            this.lblSpeed.Text = "Speed:";
            // 
            // lblFilesize
            // 
            this.lblFilesize.AutoSize = true;
            this.lblFilesize.Location = new System.Drawing.Point(32, 75);
            this.lblFilesize.Name = "lblFilesize";
            this.lblFilesize.Size = new System.Drawing.Size(44, 13);
            this.lblFilesize.TabIndex = 2;
            this.lblFilesize.Text = "Filesize:";
            // 
            // lblDownloaded
            // 
            this.lblDownloaded.AutoSize = true;
            this.lblDownloaded.Location = new System.Drawing.Point(6, 45);
            this.lblDownloaded.Name = "lblDownloaded";
            this.lblDownloaded.Size = new System.Drawing.Size(70, 13);
            this.lblDownloaded.TabIndex = 1;
            this.lblDownloaded.Text = "Downloaded:";
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(50, 15);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(26, 13);
            this.lblFile.TabIndex = 0;
            this.lblFile.Text = "File:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(463, 183);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pbLevelshot
            // 
            this.pbLevelshot.Location = new System.Drawing.Point(12, 12);
            this.pbLevelshot.Name = "pbLevelshot";
            this.pbLevelshot.Size = new System.Drawing.Size(200, 200);
            this.pbLevelshot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLevelshot.TabIndex = 2;
            this.pbLevelshot.TabStop = false;
            // 
            // frmDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(549, 218);
            this.Controls.Add(this.pbLevelshot);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gpbDownload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmDownload";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RTCW Map Downloader";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDownload_FormClosing);
            this.Load += new System.EventHandler(this.frmDownload_Load);
            this.gpbDownload.ResumeLayout(false);
            this.gpbDownload.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLevelshot)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gpbDownload;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar pbMain;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblFilesize;
        private System.Windows.Forms.Label lblDownloaded;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.PictureBox pbLevelshot;
    }
}