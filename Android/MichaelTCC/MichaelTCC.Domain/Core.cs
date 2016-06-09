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

        public JoystickBuilder JoystickBuilder { get; } = new JoystickBuilder();
        public SensorBuilder SensorBuilder { get; } = new SensorBuilder();

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

        private void TcpConnection_OnDataReceive(object sender, byte[] e)
        {
            builder.Clear();

            builder
                .AddJoystickDTO(JoystickBuilder.JoystickDTO)
                .AddSensorDTO(SensorBuilder.SensorDTO);
            ProtocolSender.Send(builder.Result(),
                                           NetworkColletion.Instance.TcpConnection);
        }
    }
}