using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MichaelTCC.Activities;
using MichaelTCC.Domain.DTO;
using MichaelTCC.Domain.Sensor;
using MichaelTCC.Fragments;
using MichaelTCC.Infrastructure.DTO;
using MichaelTCC.Service;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using MichaelTCC.Domain;

namespace MichaelTCC
{
    [Activity(Label = "MichaelTCC", MainLauncher = true, Icon = "@drawable/icon")]
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

        private void CreateSensorService()
        {
            SensorManager sensorManger = GetSystemService(SensorService) as SensorManager;
            Sensor sensor = sensorManger.GetDefaultSensor(SensorType.MagneticField);
            sensorManger.RegisterListener(this, sensor, SensorDelay.Fastest);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
            Core.Instance.SensorBuilder.SetValues(e.Values);
        }
        #endregion
    }
}

