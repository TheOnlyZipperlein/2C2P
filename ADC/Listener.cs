using _2C2P.Helper;
using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;

namespace _2C2P.ADC
{
    class Listener
    {
        public ConcurrentQueue<ImageContext> stack;
        private int port;
        private static Listener me;

        public Listener()
        {
            Init_Loop();
        }

        public static ImageContext TryGetNextImageContext()
        {
            ImageContext image=null;
            if(me.stack.Count>0)
            {
                me.stack.TryDequeue(out image);
            }
            return image;
        }

        private void Init_Loop()
        {
            me = this;
            this.port = 6475;
            stack = new ConcurrentQueue<ImageContext>();
            Thread trd = new Thread(this.loop);
            trd.Start();
        }

        private void loop()
        {
            //Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            TcpListener listener = new TcpListener(port);
            listener.Start();
            TcpClient adcClient = listener.AcceptTcpClient();
            Byte[] set = new Byte[5];
            NetworkStream stream = adcClient.GetStream();
            byte[] cmd = new Byte[1];
            byte[] size = new byte[4];
            while (adcClient.Connected)
            {
                if (stream.DataAvailable)
                {
                    stream.Read(cmd, 0, cmd.Length);                    
                    ImageRegion region = (ImageRegion)cmd[0];
                    stream.Read(size, 0, size.Length);
                    int length = 0;
                    length += ((int)((int)(size[0])) << 8);
                    length += ((int)((int)(size[1])) << 8);
                    length += ((int)((int)(size[2])) << 8);
                    length += (int) (size[3]);

                    byte[] byteBuffer = new byte[length];
                    stream.Read(byteBuffer, 0, byteBuffer.Length);
                    stack.Enqueue(new ImageContext()
                    {
                        raw = (byteBuffer),
                        region = region
                    });
                    Thread.Sleep(5);
                }
            }
        }
    }
}
