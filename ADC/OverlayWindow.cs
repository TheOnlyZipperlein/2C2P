﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using _2C2P.Helper;

namespace _2C2P.ADC
{
    
    public partial class OverlayWindow : Form
    {
        private readonly ImageConverter imageConverter = new ImageConverter();
        private static OverlayWindow me;
        private ImageContext skills;
        private ImageContext items;
        private Point skillsBase, itemsBase;

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public OverlayWindow()
        {
            InitializeComponent();
            skillsBase = new Point(676, 832);
            itemsBase = new Point(1333, 929);
            me = this;
            this.BackColor = Color.FromArgb(0, 0, 1);
            this.TransparencyKey = this.BackColor;
            this.ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.TopMost = true;
            SetWindowLong(this.Handle, -20, (int) GetWindowLong(this.Handle, -20) | 0x00000020);      
            this.Show();
        }

        internal static void DrawImageContext(ImageContext context)
        {
            switch (context.region)
            {
                case ImageRegion.items:
                    me.items = context;
                    break;
                case ImageRegion.skills:
                    me.skills = context;
                    break;
                case ImageRegion.enumNull:
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {            
            Graphics g = this.CreateGraphics();
            ImageContext context = items;
            if(context != null)
            {
                Bitmap bitmap = GetImageFromByteArray(context.raw);
                Form1.box = bitmap;
                g.DrawImage(new Bitmap(new MemoryStream(context.raw)),itemsBase);
            }
            context = skills;
            if (context != null)
            {
                MemoryStream ms = new MemoryStream(context.raw);
                Bitmap bitmap = GetImageFromByteArray(context.raw);
                g.DrawImage(new Bitmap(new MemoryStream(context.raw)), skillsBase);
            }
        }
        private void OverlayWindow_Load(object sender, EventArgs e)
        {
            
        }
        private Bitmap GetImageFromByteArray(byte[] byteArray)
        {
            Bitmap bm = (Bitmap) imageConverter.ConvertFrom(byteArray);

            if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution ||
                               bm.VerticalResolution != (int)bm.VerticalResolution))
            {
                bm.SetResolution((int)(bm.HorizontalResolution + 0.5f),
                                 (int)(bm.VerticalResolution + 0.5f));
            }

            return bm;
        }
    }
}
