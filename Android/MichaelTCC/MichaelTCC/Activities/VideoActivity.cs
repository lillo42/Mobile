using Android.App;
using Android.Content.PM;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using MichaelTCC.Domain;

namespace MichaelTCC.Activities
{
    [Activity(Label = "VideoActivity", Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen",ScreenOrientation = ScreenOrientation.Portrait)]
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                UnRegisterSensor();
            base.Dispose(disposing);
        }
        protected override void OnStart()
        {
            SensorManager sensorManger = GetSystemService(SensorService) as SensorManager;
            Sensor sensor = sensorManger.GetDefaultSensor(SensorType.Orientation);
            if (sensor != null)
                sensorManger.RegisterListener(this, sensor, SensorDelay.Fastest);
            base.OnStart();
        }

        protected override void OnPause()
        {
            RegisterSensor();
            base.OnPause();
        }

        private void RegisterSensor()
        {
            SensorManager sensorManger = GetSystemService(SensorService) as SensorManager;
            sensorManger.UnregisterListener(this);
        }

        protected override void OnStop()
        {
            UnRegisterSensor();
            base.OnStop();
        }

        private void UnRegisterSensor()
        {
            SensorManager sensorManger = GetSystemService(SensorService) as SensorManager;
            sensorManger.UnregisterListener(this);
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (Core.Instance.JoystickBuilder.AddKey(keyCode))
                return true;
            return base.OnKeyDown(keyCode, e);
        }

        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (Core.Instance.JoystickBuilder.RemoveKey(keyCode))
                return true;
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