using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using RTCWMapDownloader;
using System.Net.Http;

namespace RTCWMapDownloaderV2
{
    static class Program
    {
        //Forms
        private static frmAbout _frmAbout;
        private static frmServerPasswordManager _frmServerPasswordManager;
        private static frmDownload _frmDownload;

        //Scanners
        private static FileSystemWatcher _fswMain;

        //Other vars
        private static string _wolfExecutablePath;
        private static string _wolfMainPath;
        private static string _serverIp;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //TrayIcon & Menu
            var cmTrayMenu = new ContextMenu();
            var miAbout = new MenuItem("About");
            cmTrayMenu.MenuItems.Add(miAbout);
            miAbout.Click += miAbout_Click;

            var miServerPasswordManager = new MenuItem("Server Password Manager");
            cmTrayMenu.MenuItems.Add(miServerPasswordManager);
            miServerPasswordManager.Click += miServerPasswordManager_Click; ;

            var miExit = new MenuItem("Exit");
            cmTrayMenu.MenuItems.Add(miExit);
            miExit.Click += miExit_Click;
            var niTrayIcon = new NotifyIcon
            {
                Icon = RTCWMapDownloader.Properties.Resources.rtcw_map_downloader_ico,
                Text = "RTCW Map Downloader",
                Visible = true,
                ContextMenu = cmTrayMenu
            };

            //ProcessWatcher
            Task.Factory.StartNew(() => {
                var wolfWatcher = new WolfWatcher();
                wolfWatcher.WatchWolfProcessStart();
                wolfWatcher.WatchWolfProcessEnd();
                wolfWatcher.WolfProcessCreatedEvent += ProcessWatcher_ProcessCreated;
                wolfWatcher.WolfProcessDeletedEvent += ProcessWatcher_ProcessDeleted;

                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            });

            Application.Run();
            niTrayIcon.Visible = false;
        }

        

        private static void ProcessWatcher_ProcessDeleted(string executablePath)
        {
            if (_fswMain != null)
            {
                _fswMain.Created -= _fswMain_Created;
                _fswMain.Dispose();
                _fswMain = null;
            }
        }

        private static void ProcessWatcher_ProcessCreated(string executablePath)
        {
            _wolfExecutablePath = executablePath;
            _wolfMainPath = string.Format(@"{0}\main", Path.GetDirectoryName(executablePath));

            if (Directory.Exists(_wolfMainPath))
            {
                _fswMain = new FileSystemWatcher
                {
                    Filter = "*.pk3.tmp",
                    Path = _wolfMainPath,
                    EnableRaisingEvents = true
                };
                _fswMain.Created += _fswMain_Created;

                CleanupTempFiles();
            }
        }

        private async static void _fswMain_Created(object sender, FileSystemEventArgs e)
        {
            var createdFile = Path.GetFileNameWithoutExtension(e.Name); //Trim tmp
            var wantedFile = string.Format("{0}\\{1}", _wolfMainPath, createdFile);

            var queryUrl = $"http://breezie.be/rtcw/mapdownloader/jsonsvc.php?client=true&type=pk3&file={createdFile}";

            ServiceResponse result = null;
            var response = await Shared.HttpClient.GetAsync(queryUrl);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<ServiceResponse>();
            }

            if(result == null || result.DownloadMirrors == null || result.DownloadMirrors.Count == 0) return;

            //Get IP of current server
            _serverIp = WolfConsole.GetServerIp();
            
            //Close RTCW
            WolfConsole.QuitRtcw();

            //Download PK3
            _frmDownload = new frmDownload(result, wantedFile);
            _frmDownload.FormClosed += _frmDownload_FormClosed;
            _frmDownload.WindowState = FormWindowState.Minimized;
            _frmDownload.Show();
            _frmDownload.WindowState = FormWindowState.Normal;

            while (_frmDownload != null && _frmDownload.IsBusy)
            {
                System.Threading.Thread.Sleep(10);
                Application.DoEvents();
            }
        }

        private static void _frmDownload_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Cleanup temp files
            CleanupTempFiles();

            //Unsubscribe download form from events and dispose
            _frmDownload.FormClosed -= _frmDownload_FormClosed;
            if (_frmDownload.NoGameRestart)
            {
                _frmDownload.Dispose();
                _frmDownload = null;
                return;
            }
            _frmDownload.Dispose();
            _frmDownload = null;

            //Restart game and reconnect
            string arguments = null;
            var password = ServerPasswordManager.GetPasswordForServer(_serverIp);
            if (!string.IsNullOrWhiteSpace(password))
            {
                arguments = $"+connect {_serverIp} +password {password}";
            }
            else
            {
                arguments = $"+connect {_serverIp}";
            }

            var wolfProcess = new Process
            {
                StartInfo =
                {
                    FileName = _wolfExecutablePath,
                    WorkingDirectory = Path.GetDirectoryName(_wolfExecutablePath),
                    Arguments = arguments
                }
            };
            wolfProcess.Start();
        }

        static void miExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        static void miAbout_Click(object sender, EventArgs e)
        {
            if (_frmAbout == null)
            {
                _frmAbout = new frmAbout();
                _frmAbout.ShowDialog();
                _frmAbout.Dispose();
                _frmAbout = null;
            }
            else
            {
                _frmAbout.Activate();
                _frmAbout.BringToFront();
            }
        }

        private static void miServerPasswordManager_Click(object sender, EventArgs e)
        {
            if (_frmServerPasswordManager == null)
            {
                _frmServerPasswordManager = new frmServerPasswordManager();
                _frmServerPasswordManager.ShowDialog();
                _frmServerPasswordManager.Dispose();
                _frmServerPasswordManager = null;
            }
            else
            {
                _frmServerPasswordManager.Activate();
                _frmServerPasswordManager.BringToFront();
            }
        }

        private static void CleanupTempFiles()
        {
            if (Directory.Exists(_wolfMainPath))
            {
                foreach (var tempFile in Directory.GetFiles(_wolfMainPath))
                {
                    if (tempFile.EndsWith(".pk3.tmp"))
                    {
                        Utils.DeleteFile(tempFile);
                    }
                }
            }
        }
    }
}
