using MichaelTCC.Domain.Network;
using MichaelTCC.Infrastructure.DTO;
using MichaelTCC.Infrastructure.Network;

namespace MichaelTCC.Service
{
    public sealed class NetworkService : BaseService
    {
        public TcpServerNetwork CreateTcpConnection(ITcpConfigurationDTO tcp)
        {
            if (NetworkColletion.Instance.TcpConnection != null)
                NetworkColletion.Instance.TcpConnection.OnError -= TcpConnection_OnError;

            NetworkColletion.Instance.CreateConnection(tcp);

            NetworkColletion.Instance.TcpConnection.OnError += TcpConnection_OnError;
            return NetworkColletion.Instance.TcpConnection;
        }

        public BluetoothConnection CreateBluetooth()
        {
            NetworkColletion.Instance.CreateBluetooth();
            return NetworkColletion.Instance.BluetoothConnection;
        }

        private void TcpConnection_OnError(object sender, string e)
        {
            InvokeOnNotifition(e);
        }
    }
}