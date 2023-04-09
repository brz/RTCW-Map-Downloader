using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;

namespace RTCWMapDownloader
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://breezie.be/rtcw/mapdownloader");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try{
                var jsonResult = new WebClient().DownloadString("http://breezie.be/rtcw/mapdownloader/jsonsvc.php?type=version");
                var result = JsonConvert.DeserializeObject<string>(jsonResult);
                if (result == "1.1")
                {
                    MessageBox.Show("You are running the most recent version of RTCW Map Downloader.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex){
                MessageBox.Show(string.Format("Checking for updates failed.{0}{1}", Environment.NewLine, ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
