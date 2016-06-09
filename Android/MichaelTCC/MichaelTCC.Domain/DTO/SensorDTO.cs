using MichaelTCC.Infrastructure.DTO;

namespace MichaelTCC.Domain.DTO
{
    public class SensorDTO : ISensorDTO
    {
        public float[] SensorValues { get; set; }
    }
}