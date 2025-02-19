using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowRichPresence
{
    class ExtraUtils
    {
        [DllImport("user32", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);
        const int WM_GETTEXT = 0x000D;
        public static void Utils()
        {
            Process[] processes2 = Process.GetProcesses();
            Process[] processes = Process.GetProcessesByName("process name placeholder");
            foreach (Process p in processes)
                for (;;)
                {
                    IntPtr hwnd = p.MainWindowHandle;
                    if (hwnd == IntPtr.Zero) continue;

                    StringBuilder sb = new StringBuilder(700);

                    SendMessage(hwnd, WM_GETTEXT, sb.Capacity, sb);

                    string previous_title = null;
                    string title = sb.ToString();

                    if (previous_title != title) {
                        previous_title = title;
                        Console.WriteLine($"Current Title: {title}");
                    }
                    ;;;
                }
        }
    }
}
