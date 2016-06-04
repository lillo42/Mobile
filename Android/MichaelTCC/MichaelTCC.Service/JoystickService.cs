using MichaelTCC.Domain.Joystick;

namespace MichaelTCC.Service
{
    public sealed class JoystickService : BaseService
    {
        private readonly JoystickCapture _joystickCapture;

        public JoystickCapture JoystickCapture
        {
            get
            {
                return _joystickCapture;
            }
        }

        public JoystickService(NetworkService networkService)
        {
            _joystickCapture = new JoystickCapture(networkService.CreateBluetooth());
        }
    }
}