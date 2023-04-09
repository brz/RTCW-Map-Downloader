using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Linq;
using System.Drawing;

namespace RTCWMapDownloader
{
    public partial class frmDownload : Form
    {
        private WebClient _wc;
        private DateTime _downloadStartTime;
        private readonly string _downloadPath;
        private ServiceResponse _serviceResponse;

        public bool NoGameRestart;
        public bool IsBusy;

        public frmDownload(ServiceResponse serviceResponse, string downloadPath)
        {
            IsBusy = true;

            InitializeComponent();

            NoGameRestart = false;

            _serviceResponse = serviceResponse;
            _downloadPath = downloadPath;

            lblFile.Text = "File: " + Path.GetFileName(downloadPath);

            if (!string.IsNullOrWhiteSpace(serviceResponse.LevelshotThumb))
            {
                var levelshotByteArray = Convert.FromBase64String(serviceResponse.LevelshotThumb);
                pbLevelshot.Image = ByteToImage(levelshotByteArray);
            }
            else
            {
                pbLevelshot.Image = null;
            }

            StartDownloadAndVerification();
        }

        private static Bitmap ByteToImage(byte[] imageByteArray)
        {
            using (var mStream = new MemoryStream())
            {
                mStream.Write(imageByteArray, 0, Convert.ToInt32(imageByteArray.Length));
                Bitmap bm = new Bitmap(mStream, false);
                mStream.Dispose();
                return bm;
            }
        }

        private void StartDownloadAndVerification()
        {
            //Delete file if already existing
            if (File.Exists(_downloadPath))
            {
                Utils.DeleteFile(_downloadPath);
            }

            //Warn if all available download sources have been used once already
            if (_serviceResponse.DownloadMirrors.Where(d => d.Used == false).Count() == 0)
            {
                MessageBox.Show("All available download mirrors have been tried without success. Try downloading the file manually.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NoGameRestart = true;
                this.Close();
                return;
            }

            _wc = new WebClient();
            //Attach eventhandlers
            _wc.DownloadProgressChanged += wc_DownloadProgressChanged;
            _wc.DownloadFileCompleted += wc_DownloadFileCompleted;

            //Pick random download mirror
            var randomDownloadMirror = _serviceResponse.DownloadMirrors.Where(d => d.Used == false).ToList().PickRandom();
            randomDownloadMirror.Used = true;

            _wc.DownloadFileAsync(new Uri(randomDownloadMirror.Url), _downloadPath);
            _downloadStartTime = DateTime.Now;
        }


        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //Detach eventhandlers
            _wc.DownloadProgressChanged -= wc_DownloadProgressChanged;
            _wc.DownloadFileCompleted -= wc_DownloadFileCompleted;

            if (e.Cancelled)
            {
                return;
            }

            //Check if pk3 is valid
            if (!string.IsNullOrEmpty(_serviceResponse.Checksum) && File.Exists(_downloadPath) && Utils.CalculateMd5Checksum(_downloadPath) != _serviceResponse.Checksum)
            {
                if (MessageBox.Show("The downloaded file appears to be corrupt. Do you want to retry?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    StartDownloadAndVerification();
                    return;
                }
            }

            this.Close();
        }

        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //set labels & progressbar
            lblDownloaded.Text = string.Format("Downloaded: {0:0.00} MB", Utils.BytesToMegabytes(e.BytesReceived));
            lblFilesize.Text = string.Format("Filesize: {0:0.00} MB", Utils.BytesToMegabytes(e.TotalBytesToReceive));
            lblSpeed.Text = string.Format("Speed: {0:0} KB/s", Math.Round(Utils.BytesToKilobytes(e.BytesReceived) / ((DateTime.Now - _downloadStartTime).TotalSeconds)));
            pbMain.Value = Convert.ToInt32(Math.Round(pbMain.Maximum * e.ProgressPercentage / 100.0));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_wc.IsBusy)
            {
                _wc.CancelAsync();
                Utils.DeleteFile(_downloadPath);
                NoGameRestart = true;
                this.Close();
            }
        }

        private void frmDownload_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsBusy = false;
        }

        private void frmDownload_Load(object sender, EventArgs e)
        {

        }
    }
}
