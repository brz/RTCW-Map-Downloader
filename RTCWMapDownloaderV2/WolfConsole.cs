using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RTCWMapDownloader
{
    public class WolfConsole
    {
        #region consts
        private const int WM_GETTEXT = 0xD;
        private const int WM_GETTEXTLENGTH = 0xE;
        private const int WM_SETTEXT = 0xC;
        private const int WM_CHAR = 0x102;
        #endregion
        #region IMPORT EnumChildWindows
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowProc lpEnumFunc, IntPtr lParam);

        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);
        #endregion
        #region IMPORT GetClassName
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        #endregion
        #region IMPORT SendMessage
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, System.Text.StringBuilder lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, string lParam);
        #endregion
        #region IMPORT FindWindow
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        #endregion

        private static IntPtr[] GetChildWindows(IntPtr parentHandle)
        {
            List<IntPtr> childrenList = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(childrenList);
            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(parentHandle, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                {
                    listHandle.Free();
                }
            }

            return childrenList.ToArray();
        }

        private static bool EnumWindow(IntPtr handle, IntPtr param)
        {
            List<IntPtr> childrenList = (List<IntPtr>) GCHandle.FromIntPtr(param).Target;
            childrenList.Add(handle);

            return true;
        }

        private static IntPtr GetRtcwConsole()
        {
            return FindWindow("Wolf WinConsole", null);
        }

        private static void SendCommand(string command){
            IntPtr wolfHandle = GetRtcwConsole();
            if (wolfHandle != IntPtr.Zero)
            {
                foreach (IntPtr childHandle in GetChildWindows(wolfHandle))
                {
                    StringBuilder className = new StringBuilder(string.Empty, 256);
                    GetClassName(childHandle, className, 256);
                    if (className.ToString() == "Edit")
                    {
                        StringBuilder sb = new StringBuilder(command);
                        SendMessage(childHandle, WM_SETTEXT, 0, sb);
                        SendMessage(childHandle, WM_CHAR, 13, null);
                    }
                }
            }
        }

        private static string ReadConsole()
        {
            string consoleText = string.Empty;

            IntPtr wolfHandle = GetRtcwConsole();
            if (wolfHandle != IntPtr.Zero)
            {
                foreach (IntPtr childHandle in GetChildWindows(wolfHandle))
                {
                    StringBuilder className = new StringBuilder(string.Empty, 256);
                    GetClassName(childHandle, className, 256);
                    if (className.ToString() == "Edit")
                    {
                        IntPtr consoleTextLength = SendMessage(childHandle, WM_GETTEXTLENGTH, IntPtr.Zero, string.Empty);
                        StringBuilder sbText = new StringBuilder(consoleTextLength.ToInt32() + 1);
                        IntPtr ptrRet = SendMessage(childHandle, WM_GETTEXT, consoleTextLength.ToInt32() + 1, sbText);
                        consoleText = sbText.ToString();
                    }
                }
            }

            return consoleText;
        }

        private static void ClearConsole()
        {
            IntPtr wolfHandle = GetRtcwConsole();
            if (wolfHandle != IntPtr.Zero)
            {
                foreach (IntPtr childHandle in GetChildWindows(wolfHandle))
                {
                    StringBuilder className = new StringBuilder(string.Empty, 256);
                    GetClassName(childHandle, className, 256);
                    if (className.ToString() == "Edit")
                    {
                        IntPtr conLength = SendMessage(childHandle, WM_GETTEXTLENGTH, IntPtr.Zero, string.Empty);
                        StringBuilder sbText = new StringBuilder(conLength.ToInt32() + 1);
                        IntPtr ptrRet = SendMessage(childHandle, WM_SETTEXT, conLength.ToInt32() + 1, new StringBuilder(string.Empty));
                    }
                }
            }
        }

        public static string GetServerIp()
        {
            ClearConsole();
            System.Threading.Thread.Sleep(50);
            SendCommand("clientinfo");
            System.Threading.Thread.Sleep(50);
            var consoleContent = ReadConsole();
            System.Threading.Thread.Sleep(50);

            consoleContent = Utils.FilterIp(consoleContent);
            if (consoleContent != null)
            {
                return consoleContent;
            }
            if (GetRtcwConsole() != IntPtr.Zero)
            {
                return GetServerIp();
            }
            return null;
        }

        public static void QuitRtcw(){
            SendCommand("quit");
        }
    }
}