
using System;
using Android.App;
using Android.OS;
using Android.Widget;
using MichaelTCC.Domain.Joystick;
using MichaelTCC.Infrastructure.DTO;
using Android.Runtime;
using Android.Views;
using System.Threading;
using MichaelTCC.Domain.DTO;
using MichaelTCC.Domain;
using Android.Webkit;

namespace MichaelTCC.Activities
{
    [Activity(Label = "VideoActivity", Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
    public class VideoActivity : Activity, IJoystickCapture
    {
        private string _url;
        private readonly Semaphore _semaphore = new Semaphore(1, 1);
        private bool isFullScreen = false;
        private DateTime _lastUpdateDirection = DateTime.MinValue;
        private DateTime _lastUpdateCommand = DateTime.MinValue;
        private Keycode? _direction;
        private Keycode? _comand;

        public IJoystickDTO JoystickDTO
        {
            get
            {
                _semaphore.WaitOne();
                try
                {
                    IJoystickDTO returining = new JoystickDTO();
                    if (Math.Abs(_lastUpdateDirection.Subtract(DateTime.Now).TotalMilliseconds) < 10)
                    {
                        returining.Up = (_direction.HasValue && _direction == Keycode.DpadLeft);
                        returining.Left = (_direction.HasValue && _direction == Keycode.DpadDown);
                        returining.Right = (_direction.HasValue && _direction == Keycode.DpadUp);
                        returining.Down = (_direction.HasValue && _direction == Keycode.DpadRight);
                    }


                    if (Math.Abs(_lastUpdateCommand.Subtract(DateTime.Now).TotalMilliseconds) < 10)
                    {
                        returining.X = (_comand.HasValue && _comand == Keycode.ButtonA);
                        returining.A = (_comand.HasValue && _comand == Keycode.ButtonB);
                        returining.iOS = (_comand.HasValue && _comand == Keycode.ButtonX);
                        returining.Triangle = (_comand.HasValue && _comand == Keycode.ButtonY);
                    }

                    _comand = null;
                    _direction = null;
                    return returining;
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Core.Instance.JoystickCapture = this;

            try
            {
                _url = Intent.GetStringExtra("Url");
            }
            catch
            {
                _url = string.Empty;
            }

            SetContentView(Resource.Layout.fragment_video);

            Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.ClearFlags(WindowManagerFlags.Fullscreen);

            if (!string.IsNullOrEmpty(_url))
            {
                var v1 = FindViewById<WebView>(Resource.Id.webView1);
                v1.Settings.JavaScriptEnabled = true;
                v1.Settings.LoadsImagesAutomatically = true;
                v1.LoadUrl(_url);
            }
        }

        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {

            if (_semaphore.WaitOne(1))
            {
                try
                {
                    if (keyCode == Keycode.DpadLeft || keyCode == Keycode.DpadDown
                        || keyCode == Keycode.DpadUp || keyCode == Keycode.DpadRight)
                    {
                        _direction = keyCode;
                        _lastUpdateDirection = DateTime.Now;
                    }
                    else if (keyCode == Keycode.ButtonA || keyCode == Keycode.ButtonB
                        || keyCode == Keycode.ButtonX || keyCode == Keycode.ButtonY)
                    {
                        _comand = keyCode;
                        _lastUpdateCommand = DateTime.Now;
                    }

                }
                finally
                {
                    _semaphore.Release();
                }
            }
            return base.OnKeyUp(keyCode, e);
        }
    }
}