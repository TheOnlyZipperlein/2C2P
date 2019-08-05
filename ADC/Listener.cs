using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace _2C2P.ADC
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
            Byte[] set = new Byte[5];
            NetworkStream stream = adcClient.GetStream();
        }
    }
}
