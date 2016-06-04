namespace MichaelTCC.Infrastructure.Protocol
{
    public interface IMichaelProtocol
    {
        bool Up { get; set; }
        bool Down { get; set;}
        bool Left { get; set; }
        bool Right { get; set; }
        bool iOS { get; set; }
        bool X { get; set; }
        bool A { get; set; }
        bool Triangle { get; set; }
        float[] SensorValues { get; set; }
    }
}