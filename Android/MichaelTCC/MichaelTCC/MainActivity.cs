using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MichaelTCC.Activities;
using MichaelTCC.Domain;
using MichaelTCC.Fragments;
using MichaelTCC.Service;
using System;


namespace MichaelTCC
{
    [Activity(Label = "MichaelTCC", MainLauncher = true, Icon = "@drawable/icon",ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity, ISensorEventListener
    {
        private ConfigurationService _configService;
        private MainService _mainService;
        private NetworkService _networkService;
        private VideoService _videoService;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            CreateServices();
            CreateEvents();
            CreateConfigFragment();
            _mainService.StartServer();
        }

        private void CreateConfigFragment()
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            var configFragment = new ConfigFragment(_configService);
            transaction.Replace(Resource.Id.frameLayout, configFragment);
            transaction.Commit();
        }

        private void CreateEvents()
        {
            var btnVideo = FindViewById<Button>(Resource.Id.btnVideo);
            btnVideo.Click += BtnVideo_Click;
        }

        private void BtnVideo_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(VideoActivity));
            intent.PutExtra("Url", _videoService.GetVideoConfiguration().Url);
            UnregisterSensor();
            StartActivity(intent);
        }

        #region override
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar_main,menu);
            return base.OnCreateOptionsMenu(menu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                UnregisterSensor();
            base.Dispose(disposing);
        }

        protected override void OnStart()
        {
            RegisterSensor();
            base.OnStart();
        }

        protected override void OnPause()
        {
            UnregisterSensor();
            base.OnPause();
        }

        protected override void OnStop()
        {
            UnregisterSensor();
            base.OnStop();
        }
        #endregion

        private void RegisterSensor()
        {
            SensorManager sensorManger = GetSystemService(SensorService) as SensorManager;
            Sensor sensor = sensorManger.GetDefaultSensor(SensorType.Orientation);
            if (sensor != null)
                sensorManger.RegisterListener(this, sensor, SensorDelay.Fastest);
        }

        private void UnregisterSensor()
        {
            SensorManager sensorManger = GetSystemService(SensorService) as SensorManager;
            sensorManger.UnregisterListener(this);
        }

        #region Creating Services
        private void CreateServices()
        {
            CreateVideoService();
            CreateMainService();
        }

        private void CreateMainService()
        {
            _networkService = new NetworkService();
            _mainService = new MainService(_networkService,_configService);
            _mainService.OnNotifition += MainService_OnNotifition;
        }

        private void MainService_OnNotifition(object sender, string e)
        {
            Toast.MakeText(this, e, ToastLength.Long).Show();
        }

        private void CreateVideoService()
        {
            _configService = new ConfigurationService(ApplicationContext.FilesDir);
            _videoService = new VideoService(_configService);
        }



        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
            Core.Instance.SensorBuilder.SetValues(e.Values);
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if(Core.Instance.JoystickBuilder.AddKey(keyCode))
                return true;
            return base.OnKeyDown(keyCode, e);
        }

        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (Core.Instance.JoystickBuilder.RemoveKey(keyCode))
                return true;
            return base.OnKeyUp(keyCode, e);
        }
        #endregion
    }
}

