using _2C2P.Helper;
using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices; 
using System.Threading;
using System.Windows.Forms;

namespace _2C2P.Support
{
    class Support
    {
        [DllImport("User32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        private static Support me;

        private ConcurrentQueue<Event> events;
        public static void raiseEvent(Event e)
        {
            me.events.Enqueue(e);
        }

        public Support()
        {
            me = this;
            events = new ConcurrentQueue<Event>();
            Thread trd = new Thread(doLoop);
            trd.Start();
        }

        private void doLoop()
        {
            Event e=null;
            while (Options.NOT_CLOSED)
            {
                if (events.Count == 0)
                {
                    Thread.Sleep(5);
                }
                else
                {
                    while(events.Count > 10) events.TryDequeue(out e);

                    events.TryDequeue(out e);
                    switch(e.type)
                    {
                        case (int) type.keyUp:
                            //globalKeyboardHook.me.injectKey((Keys) e.data,(type) e.type);                            
                            break;
                        case (int) type.keyDown:
                            globalKeyboardHook.me.injectKey((Keys) e.data, (type)e.type);
                            Console.WriteLine("Key: " + ((Keys)e.data).ToString());
                            break;
                        case (int) type.mouse:
                            SetCursorPos(e.x, e.y);
                            break;
                    }
                }
            }
        }
    }
}
