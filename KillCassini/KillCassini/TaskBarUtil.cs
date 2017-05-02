using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.OLE.Interop;

namespace LatishSehgal.KillCassini
{
    internal class TaskBarUtil
    {
        public static void RefreshNotificationArea()
        {
            var notificationAreaHandle = GetNotificationAreaHandle();

            if (notificationAreaHandle == IntPtr.Zero)
                return;

            RefreshWindow(notificationAreaHandle);
        }

        private static void RefreshWindow(IntPtr windowHandle)
        {
            const uint wmMousemove = 0x0200;
			GetClientRect(windowHandle, out RECT rect);

			for (var x = 0; x < rect.right; x += 5)
                for (var y = 0; y < rect.bottom; y += 5)
                    SendMessage(
                        windowHandle,
                        wmMousemove,
                        0,
                        (y << 16) + x);
        }

        private static IntPtr GetNotificationAreaHandle()
        {
            const string notificationAreaTitle = "Notification Area";
            const string notificationAreaTitleInWindows7 = "User Promoted Notification Area";

            var systemTrayContainerHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Shell_TrayWnd", string.Empty);
            var systemTrayHandle = FindWindowEx(systemTrayContainerHandle, IntPtr.Zero, "TrayNotifyWnd", string.Empty);
            var sysPagerHandle = FindWindowEx(systemTrayHandle, IntPtr.Zero, "SysPager", string.Empty);
            var notificationAreaHandle = FindWindowEx(sysPagerHandle, IntPtr.Zero, "ToolbarWindow32", notificationAreaTitle);

            if (notificationAreaHandle == IntPtr.Zero)
                notificationAreaHandle = FindWindowEx(sysPagerHandle, IntPtr.Zero, "ToolbarWindow32",
                                                      notificationAreaTitleInWindows7);
            return notificationAreaHandle;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr handle, out RECT rect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr handle, UInt32 message, Int32 wParam, Int32 lParam);
    }
}



