using _2C2P.Helper;
using System;
using System.Windows.Forms;

namespace _2C2P
{
    public partial class Form1 : Form
    {
        private object sender;
        private object listener;
        private object role;
        public static PictureBox box;

        public Form1()
        {
            box = pictureBox1;

            Options.NOT_CLOSED = true;
            InitializeComponent();            
            globalKeyboardHook gkh = new globalKeyboardHook();

            Support.Capture cap = new Support.Capture();
            cap.capture();

            string[] args = Environment.GetCommandLineArgs();
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
            base.SetVisibleCore(true);
        }

        public void InitADC()
        {
            MouseListener mouseListener = new MouseListener();
            sender = new ADC.Sender();
            listener = new ADC.Listener();
            role = new ADC.ADC();
            Options.IS_ADC = true;
        }

        public void InitSupport()
        {
            sender = new Support.Sender();
            listener = new Support.Listener();
            role = new Support.Support();
            Options.IS_SUPPORT = true;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
