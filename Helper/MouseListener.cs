using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using _2C2P.ADC;

namespace _2C2P.Helper
{
    class MouseListener
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("kernel32.dll")]
        static extern void OutputDebugString(string lpOutputString);

        public MouseListener()
        {
            Thread trd = new Thread(doLoop);
            trd.Start();
        }

        private void doLoop()
        {
            Point cursorPos;
            while(Options.NOT_CLOSED)
            {
                GetCursorPos(out cursorPos);
                Sender.sendMousePos(cursorPos.X, cursorPos.Y);
                //Console.WriteLine(cursorPos.X + " " + cursorPos.Y);
                Thread.Sleep(5);
            }
        }      
    }
}
