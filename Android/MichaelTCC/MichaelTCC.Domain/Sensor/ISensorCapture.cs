using Android.Hardware;
using MichaelTCC.Infrastructure.DTO;

namespace MichaelTCC.Domain.Sensor
{
    public interface ISensorCapture 
    {
        ISensorDTO SensorDTO { get; }
    }
}