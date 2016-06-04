using Android.Hardware;
using MichaelTCC.Domain.Sensor;
using MichaelTCC.Infrastructure.DTO;

namespace MichaelTCC.Service
{
    public sealed class SensorService : BaseService
    {
        private readonly SensorCapture _sensorCapture = new SensorCapture();
        private readonly SensorManager _sensorManager;
        private readonly Sensor _sensor;

        public SensorCapture SensorCapture
        {
            get
            {
                return _sensorCapture;
            }
        }

        public SensorService(SensorManager sensorManager, Sensor sensor)
        {
            _sensorManager = sensorManager;
            _sensor = sensor;
        }

        public override void Pause()
        {
            _sensorManager.UnregisterListener(SensorCapture);
        }

        public override void Start()
        {
            _sensorManager.RegisterListener(SensorCapture, _sensor, SensorDelay.Normal);
        }
    }
}