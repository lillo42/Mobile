using MichaelTCC.Infrastructure.Protocol;
using MichaelTCC.Infrastructure.DTO;

namespace MichaelTCC.Domain.Protocol
{
    public sealed class MichaelProtocolBuilder
    {
        private ISensorDTO _sensorDTO;
        private IJoystickDTO _joystickDTO;

        public IMichaelProtocol Result()
        {
            IMichaelProtocol returning = new MichaelProtocol();

            SetSensor(_sensorDTO, returning);
            SetJoystick(_joystickDTO, returning);

            return returning;
        }

        private void SetSensor(ISensorDTO sensor, IMichaelProtocol protocol)
        {
            if (_sensorDTO != null)
                protocol.SensorValues = _sensorDTO.SensorValues;
        }

        private void SetJoystick(IJoystickDTO joystick, IMichaelProtocol protocol)
        {
            if (_joystickDTO != null)
            {
                protocol.Up = _joystickDTO.Up;
                protocol.Down = _joystickDTO.Down;
                protocol.Left = _joystickDTO.Left;
                protocol.Right = _joystickDTO.Right;
                protocol.A = _joystickDTO.A;
                protocol.iOS = _joystickDTO.iOS;
                protocol.Triangle = _joystickDTO.Triangle;
                protocol.X = _joystickDTO.X;
                protocol.Button1 = _joystickDTO.Button1;
            }
        }

        public void Clear()
        {
            _joystickDTO = null;
            _sensorDTO = null;
        }

        public MichaelProtocolBuilder AddSensorDTO(ISensorDTO sensorDTO)
        {
            _sensorDTO = sensorDTO;
            return this;
        }

        public MichaelProtocolBuilder AddJoystickDTO(IJoystickDTO joystickDTO)
        {
            _joystickDTO = joystickDTO;
            return this;
        }
    }
}