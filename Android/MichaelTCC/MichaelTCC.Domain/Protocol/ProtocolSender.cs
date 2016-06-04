using MichaelTCC.Infrastructure.Protocol;
using MichaelTCC.Domain.Network;
using System.Threading.Tasks;
using MichaelTCC.Infrastructure.Network;

namespace MichaelTCC.Domain.Protocol
{
    internal class ProtocolSender
    {
        internal static async Task SendAsync(IMichaelProtocol michaelProtocol, TcpServerNetwork tcpConnection)
        {
            await tcpConnection.SendMessageAsync(ConvertProtocol.MichaelProtocolToString(michaelProtocol));
        }
    }
}