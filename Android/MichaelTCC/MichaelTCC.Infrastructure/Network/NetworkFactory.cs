using Java.Util;
using System.Net;

namespace MichaelTCC.Infrastructure.Network
{
    public class NetworkFactory
    {
        public static TcpServerNetwork CreateTcp(int port)
        {
            var returning = new TcpServerNetwork(port);
            returning.StartListening();
            return returning;
        }
    }
}