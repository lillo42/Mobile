using Android.App;
using Android.Views;
using Android.OS;
using MichaelTCC.Service;
using Android.Hardware;
using Android.Widget;
using System;
using MichaelTCC.Fragments;
using Android.Content;
using MichaelTCC.Activities;
using Android.Runtime;

namespace MichaelTCC
{
    [Activity(Label = "MichaelTCC", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private ConfigurationService _configService;
        private MainService _mainService;
        private NetworkService _networkService;
        private SensorService _sensorService;
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
            StartActivity(intent);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.actionbar_main,menu);
            return base.OnCreateOptionsMenu(menu);
        }

        #region Creating Services
        private void CreateServices()
        {
            CreateSensorService();
            CreateVideoService();
            CreateMainService();
        }

        private void CreateMainService()
        {
            _networkService = new NetworkService();
            _mainService = new MainService(_networkService, _sensorService,_configService);
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

        private void CreateSensorService()
        {
            SensorManager sensorManger = GetSystemService(SensorService) as SensorManager;
            Sensor sensor = sensorManger.GetDefaultSensor(SensorType.MagneticFieldUncalibrated);
            _sensorService = new SensorService(sensorManger, sensor);
        }
        #endregion
    }
}

