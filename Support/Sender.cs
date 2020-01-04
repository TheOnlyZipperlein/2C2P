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
        public static Sender me;
        private int port;
        public ConcurrentQueue<ImageContext> stack;

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
            client.ReceiveBufferSize = 50000;
            client.SendBufferSize = 50000;
            client.GetStream();
            NetworkStream stream = client.GetStream();
            while (Options.NOT_CLOSED)
            {
                while (stack.Count > 0)
                {
                    ImageContext result = null;
                    stack.TryDequeue(out result);
                    if (stack.Count > 20)
                    {
                        for (int i = 0; i < 10; i++) stack.TryDequeue(out result);
                    }
                    Bitmap image = result.image;
                    byte[] byteBuffer = null;
                    MemoryStream memoryStream = new MemoryStream();
                    image.Save(memoryStream, ImageFormat.Tiff);
                    memoryStream.Flush();
                    byteBuffer = memoryStream.ToArray();
                    Bitmap bmp = new Bitmap(new MemoryStream(byteBuffer));
                    memoryStream.Close();
                    byte[] cmd = new byte[1];
                    byte[] size = new byte[4];
                    size[0] = (byte)(byteBuffer.Length >> 24);
                    size[1] = (byte)(byteBuffer.Length >> 16);
                    size[2] = (byte)(byteBuffer.Length >> 8);
                    size[3] = (byte)(byteBuffer.Length);
                    cmd[0] = (byte)result.region;
                    byte[] sender = new byte[byteBuffer.Length + 5];
                    cmd.CopyTo(sender, 0);
                    size.CopyTo(sender, 1);
                    byteBuffer.CopyTo(sender, 5);
                    stream.Write(sender, 0, sender.Length); 
                    byte[] ok = new byte[1];
                    while (ok[0] != 223)
                    {
                         stream.Read(ok, 0, ok.Length);
                        Thread.Sleep(5);
                    }               
                }
            }
        }
    }
}
