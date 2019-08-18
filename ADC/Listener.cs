﻿using _2C2P.Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace _2C2P.ADC
{
    class Listener
    {
        private ConcurrentQueue<ImageContext> stack;
        private int port;
        private static Listener me;

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
            TcpClient adcClient = listener.AcceptTcpClient();
            Byte[] set = new Byte[5];
            NetworkStream stream = adcClient.GetStream();
            byte[] cmd = new Byte[1];
            byte[] byteBufferItems = new Byte[131454];
            byte[] byteBufferSkills = new Byte[150006];
            while (adcClient.Connected)
            {
                if (stream.DataAvailable)
                {
                    stream.Read(cmd, 0, cmd.Length);
                    byte[] byteBuffer = null;
                    ImageRegion region=ImageRegion.enumNull;
                    switch (cmd[0])
                    {
                        case (byte)ImageRegion.skills:
                            byteBuffer = byteBufferSkills;
                            region = ImageRegion.skills;
                            break;
                        case (byte)ImageRegion.items:
                            byteBuffer = byteBufferItems;
                            region = ImageRegion.items;
                            break;
                    }
                    stream.Read(byteBuffer, 0, byteBuffer.Length);
                    stack.Enqueue(new ImageContext() {  raw = (byte[])byteBuffer.Clone(),
                                                        region = region});
                }
            }
        }
    }
}
