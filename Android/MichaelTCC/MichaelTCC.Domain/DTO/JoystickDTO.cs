using MichaelTCC.Infrastructure.DTO;
using System;

namespace MichaelTCC.Domain.DTO
{
    public class JoystickDTO : IJoystickDTO, ICloneable
    {
        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool iOS { get; set; }
        public bool X { get; set; }
        public bool A { get; set; }
        public bool Triangle { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}