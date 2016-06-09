using MichaelTCC.Domain.DTO;
using MichaelTCC.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace MichaelTCC.Domain.Sensor
{
    public sealed class SensorBuilder : ISensorCapture
    {
        private volatile IList<float> _values;
        private readonly Semaphore _semaphone = new Semaphore(1, 1);

        public ISensorDTO SensorDTO
        {
            get
            {
                _semaphone.WaitOne();
                try
                {
                    var sensor = new SensorDTO();
                    if (_values == null)
                        _values = new List<float>() { 0 };
                    sensor.SensorValues = new float[_values.Count];
                    Array.Copy(_values.ToArray(), sensor.SensorValues, _values.Count);
                    return sensor;
                }
                finally
                {
                    _semaphone.Release();
                }
            }
        }

        public void SetValues(IList<float> values)
        {
            if (_semaphone.WaitOne(1))
            {
                try
                {
                    _values = values;
                }
                finally
                {
                    _semaphone.Release();
                }
            }
        }
    }
}