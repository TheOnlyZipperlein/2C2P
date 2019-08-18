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
using EasyHook;
using System.IO;

namespace _2C2P.Support
{
    class Capture
    {
        DateTime now;
        int c;

        public static Image box;
        private CaptureProcess leagueProcess;
        private Boolean doingRequest;
        public Capture()
        {
            doingRequest = false;
            leagueProcess = attachProcess();
            now = DateTime.Now;
        }

        public void capture()
        {
            Boolean b=true;
            Thread.Sleep(3000);
            while (!doingRequest)
            {
                if(b)
                {
                    DoRequestItems();
                }
                else
                {
                    DoRequestSkills();
                }
                b = !b;
            }
        }

        private CaptureProcess attachProcess()
        {
            Config.Register("Capture", "Capture.dll");
            Process proc = Process.GetProcessesByName("League of Legends")[0];

            // If the process doesn't have a mainwindowhandle yet, skip it (we need to be able to get the hwnd to set foreground etc)
            if (proc.MainWindowHandle == IntPtr.Zero)
            {
                Thread.Sleep(10);
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

            if (reProc == null)
            {
                Thread.Sleep(10);
                return attachProcess();
            }
            else
            {
                return reProc;
            }
        }

        private void CaptureInterface_RemoteMessage(MessageReceivedEventArgs message)
        {
            //Console.WriteLine(String.Format("{0}\r\n", message));
        }

        void DoRequestSkills()
        {
            doingRequest = true;
            if (DateTime.Now.Subtract(now).TotalSeconds >= 1)
            {
                Console.WriteLine(c);
                c = 1;
                now = DateTime.Now;
            }
            else
            {
                c++;
            }
            int x = 676;
            int y = 934;
            int w = 426;
            int h = 88;
            IAsyncResult result = leagueProcess.CaptureInterface.BeginGetScreenshot(new Rectangle(x, y, w, h), new TimeSpan(0, 0, 2));
            
            while (!result.IsCompleted)
            {
                Thread.Sleep(1);
                Task.Delay(1);
            }
            Callback(result);
        }
        void Callback(IAsyncResult result)
        {
            using (Screenshot screenshot = leagueProcess.CaptureInterface.EndGetScreenshot(result))
                if (screenshot != null && screenshot.Data != null)
                    box = screenshot.ToBitmap();

            doingRequest = false;
        }
       
            
        void DoRequestItems()
        {
            doingRequest = true;
            if (DateTime.Now.Subtract(now).TotalSeconds >= 1)
            {
                Console.WriteLine(c);
                c = 1;
                now = DateTime.Now;
            }
            else
            {
                c++;
            }
            int x = 1113;
            int y = 929;
            int w = 219;
            int h = 150;
            IAsyncResult result = leagueProcess.CaptureInterface.BeginGetScreenshot(new Rectangle(x, y, w, h), new TimeSpan(0, 0, 2));

            while (!result.IsCompleted)
            {
                Thread.Sleep(1);
                Task.Delay(1);
            }
            Callback(result);
        }
    }
}
