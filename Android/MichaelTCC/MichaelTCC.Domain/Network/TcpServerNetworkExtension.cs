using MichaelTCC.Infrastructure.Network;
using System.Text;

namespace MichaelTCC.Domain.Network
{
    public static class TcpServerNetworkExtension
    {
        public static void SendMessage(this TcpServerNetwork tcp, string message)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            tcp.SendData(messageBytes);
        }
    }
}