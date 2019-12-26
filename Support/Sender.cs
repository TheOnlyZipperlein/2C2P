using _2C2P.Helper;
using System.Drawing;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using System;

namespace _2C2P.Support
{
    class Sender
    {
        private static Sender me;
        private int port;
        ConcurrentQueue<ImageContext> stack;

        public static void sendImageContext(ImageContext context)
        {
            me.sendImageContextFunction(context);
        }

        public Sender()
        {
            me = this;
            this.port = 6475;
            stack = new ConcurrentQueue<ImageContext>();
            doLoop();
        }
        private void sendImageContextFunction(ImageContext context)
        {
            stack.Enqueue(context);            
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
                    client.Connect(IPAddress.Parse(Options.IP_TO_CONNECT_TO), port);
                    Thread.Sleep(50);
                }
                catch(Exception e)
                {
                    Thread.Sleep(200);
                }
            }
            client.GetStream();
            NetworkStream stream = client.GetStream();
            while(stack.Count>0)
            {
                ImageContext result = null;
                stack.TryDequeue(out result);
                if (stack.Count > 20)
                {                    
                    for (int i = 0; i < 10; i++) stack.TryDequeue(out result);                    
                }
                Bitmap image = result.image;
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.MemoryBmp);
                byte[] byteBuffer = ms.ToArray();
                byte[] cmd = new byte[1];
                cmd[0]  = (byte) result.region;
                stream.Write(cmd, 0, cmd.Length);
                stream.Write(byteBuffer, 0, byteBuffer.Length);
            }
        }
    }
}
