using Java.Util;
using System.Net;

namespace MichaelTCC.Infrastructure.Network
{
    public class NetworkFactory
    {
        public static TcpServerNetwork CreateTcp(int port)
        {
            var returning = new TcpServerNetwork(IPAddress.Any, port);
            returning.StartListening();
            return returning;
        }
    }
}