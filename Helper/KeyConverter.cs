using System.Windows.Forms;

namespace _2C2P.Helper
{
    class KeyConverter
    {
        public static int GetKey(Keys formsKey)
        {
            /** switch (formsKey)
            {
                case Keys.D1:
                    return key._1;
                case Keys.D2:
                    return key._2;
                case Keys.D3:
                    return key._3;
                case Keys.D4:
                    return key._4;
                case Keys.D5:
                    return key._5;
                case Keys.D6:
                    return key._6;
                case Keys.D7:
                    return key._7;
                case Keys.Q:
                    return key._q;
                case Keys.W:
                    return key._w;
                case Keys.E:
                    return key._e;
                case Keys.R:
                    return key._t;
                case Keys.Z:
                    return key._z;
                case Keys.A:
                    return key._a;
                case Keys.S:
                    return key._s;
                case Keys.D:
                    return key._d;
                case Keys.F:
                    return key._f;
                case Keys.G:
                    return key._g;
                case Keys.H:
                    return key._h;
                case Keys.OemBackslash:
                    return key._arrow;
                case Keys.Y:
                    return key._y;
                case Keys.X:
                    return key._x;
                case Keys.C:
                    return key._c;
                case Keys.V:
                    return key._v;
                case Keys.B:
                    return key._b;
                case Keys.LMenu:
                    return key.alt;
                case Keys.LControlKey:
                    return key.strg;
                case Keys.LShiftKey:
                    return key.shift;
                case Keys.F1:
                    return key.F1;
                case Keys.F2:
                    return key.F2;
                case Keys.F3:
                    return key.F3;
                case Keys.F4:
                    return key.F4;
            }
            return key._empty;
            */
            return key
        }

        public static Keys GetKey(key appKey)
        {
            switch (appKey)
            {
                case key._1:
                    return Keys.D1;
                case key._2:
                    return Keys.D2;
                case key._3:
                    return Keys.D3;
                case key._4:
                    return Keys.D4;
                case key._5:
                    return Keys.D5;
                case key._6:
                    return Keys.D6;
                case key._7:
                    return Keys.D7;
                case key._q:
                    return Keys.Q;
                case key._w:
                    return Keys.W;
                case key._e:
                    return Keys.E;
                case key._t:
                    return Keys.R;
                case key._z:
                    return Keys.Z;
                case key._a:
                    return Keys.A;
                case key._s:
                    return Keys.S;
                case key._d:
                    return Keys.D;
                case key._f:
                    return Keys.F;
                case key._g:
                    return Keys.G;
                case key._h:
                    return Keys.H;
                case key._arrow:
                    return Keys.OemBackslash;
                case key._y:
                    return Keys.Y;
                case key._x:
                    return Keys.X;
                case key._c:
                    return Keys.C;
                case key._v:
                    return Keys.V;
                case key._b:
                    return Keys.B;
                case key.alt:
                    return Keys.LMenu;
                case key.strg:
                    return Keys.LControlKey;
                case key.shift:
                    return Keys.LShiftKey;
                case key.F1:
                    return Keys.F1;
                case key.F2:
                    return Keys.F2;
                case key.F3:
                    return Keys.F3;
                case key.F4:
                    return Keys.F4;
            }        
            return Keys.F5;
        }
    }
}