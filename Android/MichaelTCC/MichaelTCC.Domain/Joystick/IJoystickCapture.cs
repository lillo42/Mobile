using MichaelTCC.Infrastructure.DTO;

namespace MichaelTCC.Domain.Joystick
{
    public interface IJoystickCapture
    {
        IJoystickDTO JoystickDTO { get; }
    }
}