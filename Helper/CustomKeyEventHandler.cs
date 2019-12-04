using _2C2P.ADC;
using System.Collections.Generic;
using System.Windows.Forms;

namespace _2C2P.Helper
{
    public class CustomKeyEventHandler
    {
        List<Keys> pressedKeys;

        public CustomKeyEventHandler()
        {
            pressedKeys = new List<Keys>();
        }

        public void KeyDown(Keys key)
        {
            //Console.WriteLine("KeyDown: " + key.ToString());
            if (!pressedKeys.Contains(key) && Options.IS_ADC)
            {                
                pressedKeys.Add(key);
                Sender.sendKeyEvent(KeyConverter.GetKey(key), type.keyDown);
            }            
        }
        public void KeyUp(Keys key)
        {
            //Console.WriteLine("KeyUp: " + key.ToString());
            if (pressedKeys.Contains(key) && Options.IS_ADC)
            {                
                pressedKeys.Remove(key);
                Sender.sendKeyEvent(KeyConverter.GetKey(key), type.keyUp);
            }
        }
    }
}