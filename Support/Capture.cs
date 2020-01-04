using System;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using _2C2P.Helper;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace _2C2P.Support
{
    class Capture
    {
        public Capture()
        {
        }       


        public void capture()
        {
            Boolean b=true;
            Thread.Sleep(3000);
            while (Options.NOT_CLOSED)
            {                
                if(b)
                {
                    DoRequestItems();
                }
                else
                {
                    DoRequestSkills();
                    while(Sender.me.stack.Count>=2)
                    {
                        Thread.Sleep(10);
                    }
                    Thread.Sleep(50);
                }
                b = !b;
            }
        }

        void DoRequestSkills()
        {
            int x = 676;
            int y = 934;
            int w = 426;
            int h = 88;
            Bitmap screen = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            var gfxScreenshot = Graphics.FromImage(screen);
            gfxScreenshot.CopyFromScreen(x, y, 0, 0, new Size(w, h), CopyPixelOperation.SourcePaint);            
            Callback(screen, ImageRegion.skills);
        }           
            
        void DoRequestItems()
        {
            int x = 1113;
            int y = 929;
            int w = 219;
            int h = 150;
            Bitmap screen = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            var gfxScreenshot = Graphics.FromImage(screen);
            gfxScreenshot.CopyFromScreen(x, y, 0, 0, new Size(w, h), CopyPixelOperation.SourcePaint);
            Callback(screen, ImageRegion.items);
        }

        void Callback(Bitmap screen, ImageRegion region)
        {
            Sender.sendImageContext(new ImageContext()
            {
                image = screen,
                region = region
            });
        }
    }
}
