using _2C2P.Helper;
using System;
using System.Net.Sockets;
using System.Threading;

namespace _2C2P.Support
{
    class Listener
    {
        private int port;
        private void Init_Loop()
        {
            this.port = 6475;
            Thread trd = new Thread(this.loop);
            trd.Start();
        }

        private void loop()
        {
            //Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            TcpListener listener = new TcpListener(port);
            TcpClient adcClient = listener.AcceptTcpClient();
            Boolean b = true;
            Byte[] set = new Byte[5];
            NetworkStream stream = adcClient.GetStream();
            int value = 0, x = 0, y = 0;
            while (b)
            {
                stream.Read(set, 0, set.Length);
                if (set[0] != (byte) type.mouse)
                {
                    value = ((int)set[1]) << 24;
                    value += ((int)set[2]) << 16;
                    value += ((int)set[3]) << 8;
                    value += ((int)set[4]);
                    Console.WriteLine("Key-Value " + value + "recieved!");
                    Support.raiseEvent(new Event(set[0], value));
                }
                else
                {
                    x += ((int)set[1]) << 8;
                    x += ((int)set[2]);
                    y += ((int)set[3]) << 8;
                    y += ((int)set[4]);
                    Console.WriteLine("Mouseposition " + x + ":" + y + "recieved!");
                    Support.raiseEvent(new Event(set[0], x, y));
                }
            }
        }
    }
}
