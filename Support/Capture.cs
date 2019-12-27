using System;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using _2C2P.Helper;
using System.Windows.Forms;
using System.Drawing.Imaging;

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
            Bitmap screen = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            var gfxScreenshotSkills = Graphics.FromImage(screen);
            gfxScreenshotSkills.CopyFromScreen(x, y, 0, 0, new Size(w, h), CopyPixelOperation.SourceCopy);
            Callback(screen, ImageRegion.skills);
        }           
            
        void DoRequestItems()
        {
            int x = 1113;
            int y = 929;
            int w = 219;
            int h = 150;
            Bitmap screen = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            var gfxScreenshotItems = Graphics.FromImage(screen);
            gfxScreenshotItems.CopyFromScreen(x, y, 0, 0, new Size(w, h), CopyPixelOperation.SourceCopy);
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
