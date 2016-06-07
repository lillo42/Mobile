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