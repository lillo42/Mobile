using Android.Views;
using MichaelTCC.Domain.DTO;
using MichaelTCC.Infrastructure.DTO;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MichaelTCC.Domain.Joystick
{
    public sealed class JoystickBuilder : IJoystickCapture
    {
        private readonly Semaphore _semaphore = new Semaphore(1, 1);
        private readonly ReadOnlyCollection<Keycode> _keys;
        private List<Keycode> _listComand = new List<Keycode>();

        public JoystickBuilder()
        {
            _keys = new ReadOnlyCollection<Keycode>(new List<Keycode> {
                Keycode.ButtonA,
                Keycode.ButtonB,
                Keycode.ButtonX,
                Keycode.ButtonY,
                Keycode.DpadLeft,
                Keycode.DpadDown,
                Keycode.DpadUp,
                Keycode.DpadUp,
                Keycode.MediaFastForward,
                Keycode.MediaRewind,
                Keycode.MediaNext,
                Keycode.MediaPrevious,
                Keycode.VolumeUp,
                Keycode.VolumeDown,
                Keycode.Back
            });
        }

        public bool RemoveKey(Keycode keyCode)
        {
            bool returning = false;
            if (_keys.Contains(keyCode))
            {
                returning = true;
                if (_semaphore.WaitOne(1))
                {
                    try
                    {
                        if (_listComand.Exists(x => x == keyCode))
                            _listComand.Remove(keyCode);

                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
            }
            return returning;
        }

        public bool AddKey(Keycode keyCode)
        {
            bool returning = false;
            if (_keys.Contains(keyCode))
            {
                returning = true;
                if (_semaphore.WaitOne(1))
                {
                    try
                    {
                        if (!_listComand.Exists(x => x == keyCode))
                            _listComand.Add(keyCode);
                    }
                    finally
                    {
                        _semaphore.Release();
                    }

                }
            }
            return returning;
        }

        public IJoystickDTO JoystickDTO
        {
            get
            {
                _semaphore.WaitOne();
                try
                {
                    IJoystickDTO returining = new JoystickDTO();

                    returining.Up = _listComand.Exists(x => x == Keycode.DpadLeft) || _listComand.Exists(x => x == Keycode.MediaFastForward);
                    returining.Left = _listComand.Exists(x => x == Keycode.DpadDown) || _listComand.Exists(x => x == Keycode.MediaPrevious);
                    returining.Right = _listComand.Exists(x => x == Keycode.DpadUp) || _listComand.Exists(x => x == Keycode.MediaNext);
                    returining.Down = _listComand.Exists(x => x == Keycode.DpadRight) || _listComand.Exists(x => x == Keycode.MediaRewind);



                    returining.X = _listComand.Exists(x => x == Keycode.ButtonA) || _listComand.Exists(x => x == Keycode.VolumeDown);
                    returining.A = _listComand.Exists(x => x == Keycode.ButtonB);
                    returining.iOS = _listComand.Exists(x => x == Keycode.ButtonX) || _listComand.Exists(x => x == Keycode.VolumeUp);
                    returining.Triangle = _listComand.Exists(x => x == Keycode.ButtonY) || _listComand.Exists(x => x == Keycode.Back);

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