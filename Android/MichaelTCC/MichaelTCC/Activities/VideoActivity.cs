using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MichaelTCC.Domain;
using Android.Webkit;
using Android.Hardware;
using System;

namespace MichaelTCC.Activities
{
    [Activity(Label = "VideoActivity", Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
    public class VideoActivity : Activity, ISensorEventListener
    {
        private string _url;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

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

        private void CreateSensorService()
        {
            SensorManager sensorManger = GetSystemService(SensorService) as SensorManager;
            Sensor sensor = sensorManger.GetDefaultSensor(SensorType.MagneticField);
            sensorManger.RegisterListener(this, sensor, SensorDelay.Fastest);
        }

        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            Core.Instance.JoystickBuilder.SetDirection(keyCode);
            Core.Instance.JoystickBuilder.SetCommand(keyCode);
            return base.OnKeyUp(keyCode, e);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
        }

        public void OnSensorChanged(SensorEvent e)
        {
            Core.Instance.SensorBuilder.SetValues(e.Values);
        }
    }
}