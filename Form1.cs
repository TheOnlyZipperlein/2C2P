using _2C2P.ADC;
using _2C2P.Helper;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace _2C2P
{
    public partial class Form1 : Form
    {
        private object sender;
        private object listener;
        private object role;
        private _2C2P.ADC.Overlay overlay;
        public static Image box;
        globalKeyboardHook gkh;
        Thread trd;

        public Form1()
        {
            InitializeComponent();            
            SubInitFrom1();
        }

        private void SubInitFrom1()
        {
            Options.NOT_CLOSED = true;
            gkh = new globalKeyboardHook();

            string[] args = Environment.GetCommandLineArgs();
            args = new String[2];
            args[0] = "adc";
            args[1] = "192.168.178.33";
            if (args.Length == 2)
            {
                Options.IP_TO_CONNECT_TO = args[1];
                switch (args[0])
                {
                    case "adc":
                        InitADC();
                        break;
                    case "sup":
                        InitSupport();
                        break;
                }
            }
        }
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(value);
            //base.SetVisibleCore(false);
        }

        public void InitADC()
        {
            MouseListener mouseListener = new MouseListener();
            OverlayWindow aha = new OverlayWindow();
            sender = new ADC.Sender();
            listener = new ADC.Listener();
            role = new ADC.ADC();
            overlay = new ADC.Overlay();
            Options.IS_ADC = true;
        }

        public void InitSupport()
        {
            Support.Capture cap = new Support.Capture();            
            Options.IS_SUPPORT = true;
            sender = new Support.Sender();
            listener = new Support.Listener();
            role = new Support.Support();
            Options.IS_SUPPORT = true;

            trd = new Thread(new ThreadStart(cap.capture));
            trd.Start();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
