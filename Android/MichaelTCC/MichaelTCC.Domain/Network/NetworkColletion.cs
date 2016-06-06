using MichaelTCC.Infrastructure.Network;
using MichaelTCC.Infrastructure.DTO;
using Java.Util;

namespace MichaelTCC.Domain.Network
{
    public class NetworkColletion
    {
        private NetworkColletion(){}
        private static NetworkColletion _instance;
        private static object locker = new object();
        public TcpServerNetwork TcpConnection { get; private set; }
        public BluetoothConnection BluetoothConnection { get; private set; }
        private const string JOYSTICK_NAME = "MOCUTE-032_B53-B3A1";
        private static UUID JOYSTICK_UUID = UUID.FromString("00001124-0000-1000-8000-00805f9b34fb");

        public static NetworkColletion Instance
        {
            get
            {
                if(_instance == null)
                {
                    lock(locker)
                    {
                        if (_instance == null)
                            _instance = new NetworkColletion();
                    }
                }
                return _instance;
            }
        }

        public void CreateConnection(ITcpConfigurationDTO tcpConfiguration)
        {
            StopConnection();
            TcpConnection = NetworkFactory.CreateTcp(tcpConfiguration.Port);
        }

        public void CreateBluetooth()
        {
            BluetoothConnection = NetworkFactory.CreateBluetooth(JOYSTICK_NAME, JOYSTICK_UUID);
        }

        public void StopConnection()
        {
            if (TcpConnection != null)
            {
                TcpConnection.Stop();
                TcpConnection.Dispose();
            }
        }
    }
}