using MichaelTCC.Infrastructure.Protocol;

namespace MichaelTCC.Domain.Protocol
{
    public class DataReceiveProtocol : IDataReceiveProtocol
    {
        public DataReceiveProtocol()
        {
            Campo1 = string.Empty;
            Campo2 = string.Empty;
            Campo3 = string.Empty;
            Campo4 = string.Empty;
            Campo5 = string.Empty;
        }

        public string Campo1 { get; set; }
        public string Campo2 { get; set; }
        public string Campo3 { get; set; }
        public string Campo4 { get; set; }
        public string Campo5 { get; set; }
    }
}