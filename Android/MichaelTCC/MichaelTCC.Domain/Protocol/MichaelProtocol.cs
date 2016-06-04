using MichaelTCC.Infrastructure.Protocol;

namespace MichaelTCC.Domain.Protocol
{
    public class MichaelProtocol : IMichaelProtocol
    {
        public bool Down { get; set; }
        public bool Left { get; set; }
        public bool Up { get; set; }
        public bool Right { get; set; }
        public bool iOS { get; set; }
        public bool X { get; set; }
        public bool A { get; set; }
        public bool Triangle { get; set; }
        public float[] SensorValues { get; set; }
    }
}