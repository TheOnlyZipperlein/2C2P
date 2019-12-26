using _2C2P.Helper;
using System;
using System.Drawing;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using System.Windows.Input;

namespace _2C2P.ADC
{
    class Sender
    {
        private static Sender me;
        private int port;
        public ConcurrentQueue<Event> stack;

        public static void sendKeyEvent(Keys keyValue, type keyType)
        {
            if(me!=null) me.stack.Enqueue(new Event((int) keyType, (int) keyValue));
        }

        public static void sendMousePos(int x, int y)
        {
            if (me != null) me.stack.Enqueue(new Event((int)type.mouse, x, y));            
        }

        public Sender()
        {
            this.port = 6475;
            stack = new ConcurrentQueue<Event>();
            me = this;
            doLoop();
        }
        private void doLoop()
        {
            Thread trd = new Thread(loop);
            trd.Start();
        }

        private void loop()
        {
            TcpClient client = new TcpClient();
            while (!client.Connected)
            {
                try
                {
                    IPAddress ip = IPAddress.Parse(Options.IP_TO_CONNECT_TO);
                    client.Connect(ip, port);
                    Console.WriteLine("Sender connected!");
                }
                catch (Exception e)
                {
                    //TODO
                }
                Thread.Sleep(50);
            }
            NetworkStream stream = client.GetStream();
            Boolean b = true;
            Byte[] buffer = new Byte[5];
            while (b)
            {
                if (stack.Count > 0)
                {
                    Event e = null;
                    while (e == null)
                    {
                        stack.TryDequeue(out e);
                        if (e == null) Thread.Sleep(5);
                    }
                    buffer[0] = (byte)e.type;
                    if (e.type != (int) type.mouse)
                    {
                        buffer[1] = (byte)(e.data >> 24);
                        buffer[2] = (byte)(e.data >> 16);
                        buffer[3] = (byte)(e.data >> 8);
                        buffer[4] = (byte)(e.data);
                    }
                    else
                    {
                        buffer[1] = (byte)(e.x >> 8);
                        buffer[2] = (byte)(e.x);
                        buffer[3] = (byte)(e.y >> 8);
                        buffer[4] = (byte)(e.y);
                    }
                    stream.Write(buffer, 0, buffer.Length);
                    e = null;
                }
            }

        }
    }
}
