using System;
using MichaelTCC.Infrastructure.Network;
using System.Threading.Tasks;
using System.Text;

namespace MichaelTCC.Domain.Network
{
    public static class TcpServerNetworkExtension
    {
        public static async Task  SendMessageAsync(this TcpServerNetwork tcp, string message)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            await tcp.SendDataAsync(messageBytes);
        }
    }
}