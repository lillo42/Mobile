using Android.Runtime;
using Android.Hardware;
using MichaelTCC.Infrastructure.DTO;
using MichaelTCC.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MichaelTCC.Domain.Sensor
{
    public class SensorCapture : ISensorEventListener
    {
        private volatile IList<float> values;
        private bool isDispose = true;
        private readonly Semaphore _semaphone = new Semaphore(1, 1);

        public ISensorDTO Sensor
        {
            get
            {
                _semaphone.WaitOne();
                try
                {
                    var sensor = new SensorDTO();
                    if (values == null)
                        values = new List<float>() { 0 };
                    sensor.SensorValues = new float[values.Count];
                    Array.Copy(values.ToArray(), sensor.SensorValues, values.Count);
                    return sensor;
                }
                finally
                {
                    _semaphone.Release();
                }
            }
        }

        public IntPtr Handle
        {
            get
            {
                return IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            Dispose(isDispose);
            isDispose = false;
        }

        public void Dispose(bool dispose)
        {
            if(dispose)
                _semaphone.Dispose();
        }

        public void OnAccuracyChanged(Android.Hardware.Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (_semaphone.WaitOne(1))
            {
                try
                {
                    values = e.Values;
                }
                finally
                {
                    _semaphone.Release();
                }
            }
        }
    }
}