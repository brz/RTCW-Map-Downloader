using System.Management;

namespace RTCWMapDownloader
{
    internal class WolfWatcher
    {
        public delegate void WolfProcessCreatedEventHandler(string executablePath);
        public event WolfProcessCreatedEventHandler WolfProcessCreatedEvent;

        public delegate void WolfProcessDeletedEventHandler(string executablePath);
        public event WolfProcessDeletedEventHandler WolfProcessDeletedEvent;

        public ManagementEventWatcher WatchWolfProcessStart()
        {
            string queryString =
                "SELECT TargetInstance" +
                "  FROM __InstanceCreationEvent " +
                "WITHIN  1 " +
                " WHERE TargetInstance ISA 'Win32_Process' " +
                "   AND TargetInstance.Name = 'WolfMP.exe'";

            // The dot in the scope means use the current machine
            string scope = @"\\.\root\CIMV2";

            // Create a watcher and listen for events
            ManagementEventWatcher watcher = new ManagementEventWatcher(scope, queryString);
            watcher.EventArrived += ProcessStarted;
            watcher.Start();
            return watcher;
        }

        public ManagementEventWatcher WatchWolfProcessEnd()
        {
            string queryString =
                "SELECT TargetInstance" +
                "  FROM __InstanceDeletionEvent " +
                "WITHIN  1 " +
                " WHERE TargetInstance ISA 'Win32_Process' " +
                "   AND TargetInstance.Name = 'WolfMP.exe'";

            // The dot in the scope means use the current machine
            string scope = @"\\.\root\CIMV2";

            // Create a watcher and listen for events
            ManagementEventWatcher watcher = new ManagementEventWatcher(scope, queryString);
            watcher.EventArrived += ProcessEnded;
            watcher.Start();
            return watcher;
        }

        private void ProcessStarted(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
            string executablePath = targetInstance.Properties["ExecutablePath"].Value.ToString();
            WolfProcessCreatedEvent?.Invoke(executablePath);
        }

        private void ProcessEnded(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;
            string executablePath = targetInstance.Properties["ExecutablePath"].Value.ToString();
            WolfProcessDeletedEvent?.Invoke(executablePath);
            
        }
    }
}
