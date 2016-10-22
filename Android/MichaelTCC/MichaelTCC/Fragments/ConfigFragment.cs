using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using MichaelTCC.Service;
using Android.App;
using MichaelTCC.Domain.DTO;
using MichaelTCC.Infrastructure.DTO;

namespace MichaelTCC.Fragments
{
    public class ConfigFragment : Fragment
    {
        private EditText txtUrl { get; set; }
        private EditText txtPort { get; set; }
        private EditText txtTempo { get; set; }
        private Button btnSave { get; set; }

        private ConfigurationService _configservice;

        public ConfigFragment()
        { }

        public ConfigFragment(ConfigurationService configservice)
        {
            _configservice = configservice;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_config, container, false);

            txtUrl = view.FindViewById<EditText>(Resource.Id.edtUrl);
            txtPort= view.FindViewById<EditText>(Resource.Id.edtPort);
            txtTempo = view.FindViewById<EditText>(Resource.Id.edtTempo);
            btnSave = view.FindViewById<Button>(Resource.Id.btnSave);

            UpdateConfig();

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveVideoConfig();
            SaveTcpConfig();
            UpdateConfig();
        }

        private void UpdateConfig()
        {
            UpdateVideoConfig();
            UpdateTcpConfig();
        }

        private void UpdateTcpConfig()
        {
            ITcpConfigurationDTO tcpDTO = _configservice.ReadTcp();
            txtPort.Text = tcpDTO.Port.ToString();
            txtTempo.Text = tcpDTO.Time.ToString();
        }

        private void UpdateVideoConfig()
        {
            IVideoConfigurationDTO videoDTO = _configservice.ReadVideo();
            txtUrl.Text = videoDTO.Url;
        }

        private void SaveVideoConfig()
        {
            _configservice.Save(new VideoConfigurationDTO { Url = txtUrl.Text });
        }

        private void SaveTcpConfig()
        {
            int port;
            if (!int.TryParse(txtPort.Text, out port))
            {
                port = 8000;
                txtPort.Text = "8000";
            }

            int time;
            if(!int.TryParse(txtTempo.Text,out time))
            {
                time = 100;
                txtTempo.Text = "100";
            }

            _configservice.Save(new TcpConfigurationDTO { Port = port, Time = time });
        }
    }
}