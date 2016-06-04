using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MichaelTCC.Domain.Sensor;
using MichaelTCC.Domain.Joystick;
using MichaelTCC.Infrastructure.DTO;
using MichaelTCC.Domain.Network;
using MichaelTCC.Domain.Protocol;

namespace MichaelTCC.Domain
{
    public class Core
    {
        private Core() { }
        private static object locker = new object();
        private static Core _instante;

        public static Core Instance
        {
            get
            {
                if (_instante == null)
                {
                    lock (locker)
                    {
                        if (_instante == null)
                            _instante = new Core();
                    }
                }
                return _instante;
            }
        }

        public SensorCapture SensorCapture { get; set; }
        public JoystickCapture JoystickCapture { get; set; }
        private readonly MichaelProtocolBuilder builder = new MichaelProtocolBuilder();

        public void StartServerTcp(ITcpConfigurationDTO tcp)
        {
            UpdateTcpConfiguration(tcp);
        }

        internal void UpdateTcpConfiguration(ITcpConfigurationDTO tcp)
        {
            DettachedEvent();
            NetworkColletion.Instance.CreateConnection(tcp);
            AttachedEvent();
        }

        private void DettachedEvent()
        {
            if (NetworkColletion.Instance.TcpConnection != null)
                NetworkColletion.Instance.TcpConnection.OnDataReceive -= TcpConnection_OnDataReceive;
        }

        private void AttachedEvent()
        {
            if (NetworkColletion.Instance.TcpConnection != null)
                NetworkColletion.Instance.TcpConnection.OnDataReceive += TcpConnection_OnDataReceive;
        }

        private async void TcpConnection_OnDataReceive(object sender, byte[] e)
        {
            builder.Clear();
            await ProtocolSender.SendAsync(builder
                                            .AddJoystickDTO(JoystickCapture.JoystickDTO)
                                            .AddSensorDTO(SensorCapture.Sensor)
                                            .Result(),
                                           NetworkColletion.Instance.TcpConnection);
        }
    }
}