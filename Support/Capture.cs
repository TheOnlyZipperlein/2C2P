using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capture.Interface;
using Capture.Hook;
using Capture;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace _2C2P.Support
{
    class Capture
    {
        private CaptureProcess leagueProcess;
        public Capture()
        {
            leagueProcess = attachProcess();
        }

        public void capture()
        {
            DoRequest();
        }

        private CaptureProcess attachProcess()
        {
            Process proc = Process.GetProcessesByName("League of Legends")[0];

            // If the process doesn't have a mainwindowhandle yet, skip it (we need to be able to get the hwnd to set foreground etc)
            if (proc.MainWindowHandle == IntPtr.Zero)
            {
                return attachProcess();
            }

            // Skip if the process is already hooked (and we want to hook multiple applications)
            if (HookManager.IsHooked(proc.Id))
            {
                return leagueProcess;
            }

            Direct3DVersion direct3DVersion = Direct3DVersion.Direct3D9;

            CaptureConfig cc = new CaptureConfig()
            {
                Direct3DVersion = direct3DVersion,
            };

            var captureInterface = new CaptureInterface();
            captureInterface.RemoteMessage += new MessageReceivedEvent(CaptureInterface_RemoteMessage);
            CaptureProcess reProc = new CaptureProcess(proc, cc, captureInterface);

            Thread.Sleep(10);

            if (leagueProcess == null)
            {
                return attachProcess();
            }
            else
            {
                return reProc;
            }
        }

        private void CaptureInterface_RemoteMessage(MessageReceivedEventArgs message)
        {
            Console.WriteLine(String.Format("{0}\r\n{1}", message));
        }

        void DoRequest()
        {
            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;
            leagueProcess.CaptureInterface.BeginGetScreenshot(new Rectangle(x, y, w, h), new TimeSpan(0, 0, 2), Callback);
        }
        void Callback(IAsyncResult result)
        {
            using (Screenshot screenshot = leagueProcess.CaptureInterface.EndGetScreenshot(result))
                try
                {
                    if (screenshot != null && screenshot.Data != null)
                    {

                        Form1.box.Image = screenshot.ToBitmap();
                    }
                }
                catch
                {
                    Console.WriteLine("Failed getting Screenshot!");
                }
        }
    }
}
