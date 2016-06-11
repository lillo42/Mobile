using MichaelTCC.Infrastructure.Protocol;
using MichaelTCC.Domain.Network;
using System.Threading.Tasks;
using MichaelTCC.Infrastructure.Network;

namespace MichaelTCC.Domain.Protocol
{
    internal class ProtocolSender
    {
        internal static void Send(IMichaelProtocol michaelProtocol, TcpServerNetwork tcpConnection)
        {
            string strSend = ConvertProtocol.MichaelProtocolToString(michaelProtocol);
            tcpConnection.SendMessage(strSend);
        }
    }
}