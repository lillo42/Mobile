using System;
using Android.Content;
using Android.Views;
using Android.Webkit;
using Java.Interop;

namespace MichaelTCC.WebInterface
{
    public class KeyPressWebInterface : BaseWebInterface
    {
        public KeyPressWebInterface(Context context) : base(context)
        {
        }

        public event EventHandler<Keycode> OnKeyUp;
        public event EventHandler<Keycode> OnKeyDown;

        [Export]
        [JavascriptInterface]
        public void keyUp(int keyCode)
        {
            OnKeyUp?.Invoke(this, Transalate(keyCode));
        }


        [Export]
        [JavascriptInterface]
        public void keyDown(int keyCode)
        {
            OnKeyDown?.Invoke(this, Transalate(keyCode));
        }

        private Keycode Transalate(int keyCode)
        {
            switch(keyCode)
            {
                case 228:
                    return Keycode.DpadUp;
                case 227:
                    return Keycode.DpadDown;
                case 177:
                    return Keycode.DpadLeft;
                case 176:
                    return Keycode.DpadRight;
                case 13:
                    return Keycode.ButtonB;
                default:
                    return Keycode.D;
            }
        }
    }
}