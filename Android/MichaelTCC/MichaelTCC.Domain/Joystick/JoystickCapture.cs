using System;
using MichaelTCC.Infrastructure.DTO;
using MichaelTCC.Infrastructure.Network;
using MichaelTCC.Domain.DTO;

namespace MichaelTCC.Domain.Joystick
{
    public class JoystickCapture
    {
        private readonly BluetoothConnection _connection;

        public JoystickCapture(BluetoothConnection connection)
        {
            _connection = connection;
            _connection.OnDataReceive += Connection_OnDataReceive;
        }

        private void Connection_OnDataReceive(object sender, byte[] e)
        {

        }

        public IJoystickDTO JoystickDTO
        {
            get
            {
                return new JoystickDTO();
            }
        }
    }
}