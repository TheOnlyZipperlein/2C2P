using System.Threading;
using _2C2P.Helper;
namespace _2C2P.ADC
{
    public class Overlay
    {
        public static Overlay me;

        public Overlay()
        {
            Thread drawLoop = new Thread(this.loop);
            drawLoop.Start();
        }

        public void loop()
        {
            while(true)
            {
                DrawNext();
            }
        }

        public void DrawNext()
        {
            ImageContext context = Listener.TryGetNextImageContext();
            if (context !=null)
            {
                OverlayWindow.DrawImageContext(context);
            }
            else
            {
                Thread.Sleep(50);
            }
        }


    }
}