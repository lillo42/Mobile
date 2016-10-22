using MichaelTCC.Infrastructure.DTO;

namespace MichaelTCC.Domain.DTO
{
    public class TcpConfigurationDTO : ITcpConfigurationDTO
    {
        public int Port { get; set; }
        public int Time { get; set; }
    }
}