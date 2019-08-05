using _2C2P.Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _2C2P.Support
{
    class Support
    {
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
                    events.TryDequeue(out e);
                    switch(e.type)
                    {
                        case (int) type.keyUp:
                            globalKeyboardHook.injectKey(KeyConverter.GetKey((key) e.data),(type) e.type);
                            break;
                        case (int) type.keyDown:
                            globalKeyboardHook.injectKey(KeyConverter.GetKey((key)e.data), (type)e.type);
                            break;
                        case (int) type.mouse:
                            
                            break;
                    }
                }
            }
        }
    }
}
