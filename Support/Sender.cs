using _2C2P.Helper;
using System;
using System.Drawing;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net;

namespace _2C2P.Support
{
    class Sender
    {
        private static Sender me;
        private int port;
        ConcurrentStack<Event> stack;

        public static void sendPicture(Bitmap[] images)
        {            
            me.sendPicture(images[0]);
        }

        public Sender()
        {
            me = this;
            this.port = 6475;
            stack = new ConcurrentStack<Event>();
        }
        private void sendPicture(Bitmap image)
        {

        }

        private void doLoop()
        {
            Thread trd = new Thread(loop);
            trd.Start();
        }

        private void loop()
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse("192.168.178.X"), port);
            client.GetStream();
        }
    }
}
