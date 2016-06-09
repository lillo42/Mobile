using Android.Views;
using MichaelTCC.Domain.DTO;
using MichaelTCC.Infrastructure.DTO;
using System;
using System.Threading;

namespace MichaelTCC.Domain.Joystick
{
    public  sealed class JoystickBuilder : IJoystickCapture
    {
        private readonly Semaphore _semaphore = new Semaphore(1, 1);
        private DateTime _lastUpdateDirection = DateTime.MinValue;
        private DateTime _lastUpdateCommand = DateTime.MinValue;
        private Keycode? _direction;
        private Keycode? _comand;

        public void SetDirection(Keycode keyCode)
        {
            if (_semaphore.WaitOne(1))
            {
                try
                {
                    if (keyCode == Keycode.DpadLeft || keyCode == Keycode.DpadDown
                                || keyCode == Keycode.DpadUp || keyCode == Keycode.DpadRight)
                    {
                        _direction = keyCode;
                        _lastUpdateDirection = DateTime.Now;
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }

        public void SetCommand(Keycode keyCode)
        {
            if (_semaphore.WaitOne(1))
            {
                try
                {
                    if (keyCode == Keycode.ButtonA || keyCode == Keycode.ButtonB
                        || keyCode == Keycode.ButtonX || keyCode == Keycode.ButtonY)
                    {
                        _comand = keyCode;
                        _lastUpdateCommand = DateTime.Now;
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }

        public IJoystickDTO JoystickDTO
        {
            get
            {
                _semaphore.WaitOne();
                try
                {
                    IJoystickDTO returining = new JoystickDTO();
                    if (Math.Abs(_lastUpdateDirection.Subtract(DateTime.Now).TotalMilliseconds) < 10)
                    {
                        returining.Up = (_direction.HasValue && _direction == Keycode.DpadLeft);
                        returining.Left = (_direction.HasValue && _direction == Keycode.DpadDown);
                        returining.Right = (_direction.HasValue && _direction == Keycode.DpadUp);
                        returining.Down = (_direction.HasValue && _direction == Keycode.DpadRight);
                    }


                    if (Math.Abs(_lastUpdateCommand.Subtract(DateTime.Now).TotalMilliseconds) < 10)
                    {
                        returining.X = (_comand.HasValue && _comand == Keycode.ButtonA);
                        returining.A = (_comand.HasValue && _comand == Keycode.ButtonB);
                        returining.iOS = (_comand.HasValue && _comand == Keycode.ButtonX);
                        returining.Triangle = (_comand.HasValue && _comand == Keycode.ButtonY);
                    }

                    _comand = null;
                    _direction = null;
                    return returining;
                }
                finally
                {
                    _semaphore.Release();
                }
            }
        }
    }
}