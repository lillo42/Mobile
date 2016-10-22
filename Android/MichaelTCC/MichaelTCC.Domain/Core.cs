using MichaelTCC.Domain.Sensor;
using MichaelTCC.Domain.Joystick;
using MichaelTCC.Infrastructure.DTO;
using MichaelTCC.Domain.Network;
using MichaelTCC.Domain.Protocol;
using System.Threading;
using System.Threading.Tasks;

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
        public DataReceiveController DataReceive { get; } = new DataReceiveController();

        private readonly MichaelProtocolBuilder builder = new MichaelProtocolBuilder();
        private readonly TaskFactory _taskFactory = new TaskFactory();
        private ITcpConfigurationDTO _tcpDTO;
        private CancellationTokenSource _cancel;

        public void StartServerTcp(ITcpConfigurationDTO tcp)
        {
            UpdateTcpConfiguration(tcp);
        }

        internal void UpdateTcpConfiguration(ITcpConfigurationDTO tcp)
        {
            DettachedEvent();
            NetworkColletion.Instance.CreateConnection(tcp);
            AttachedEvent(tcp);
            _tcpDTO = tcp;
        }

        private void SendInfoTask(ITcpConfigurationDTO tcp, CancellationToken token)
        {
            if (tcp.Time >= 0)
            {
                while (!token.IsCancellationRequested)
                {
                    SendInfo();
                    Task.Delay(tcp.Time, token).Wait();
                }
            }
        }

        private void SendInfo()
        {
            builder.Clear();
            builder
                .AddJoystickDTO(JoystickBuilder.JoystickDTO)
                .AddSensorDTO(SensorBuilder.SensorDTO);
            ProtocolSender.Send(builder.Result(), NetworkColletion.Instance.TcpConnection);
        }

        private void DettachedEvent()
        {
            if (_cancel != null)
                _cancel.Cancel();
            _cancel = new CancellationTokenSource();
            if (NetworkColletion.Instance.TcpConnection != null)
                NetworkColletion.Instance.TcpConnection.OnDataReceive -= TcpConnection_OnDataReceive;
        }

        private void AttachedEvent(ITcpConfigurationDTO tcp)
        {
            if (NetworkColletion.Instance.TcpConnection != null)
                NetworkColletion.Instance.TcpConnection.OnDataReceive += TcpConnection_OnDataReceive;
            _taskFactory.StartNew(() => SendInfoTask(tcp, _cancel.Token));
        }

        private void TcpConnection_OnDataReceive(object sender, byte[] e)
        {
            DataReceive.SetDataProtocol(ConvertProtocol.BytesToIDataReceiveProtocol(e));

            if (_tcpDTO.Time < 0)
            {
                builder.Clear();
                builder
                    .AddJoystickDTO(JoystickBuilder.JoystickDTO)
                    .AddSensorDTO(SensorBuilder.SensorDTO);
                ProtocolSender.Send(builder.Result(), NetworkColletion.Instance.TcpConnection);
            }
        }
    }
}