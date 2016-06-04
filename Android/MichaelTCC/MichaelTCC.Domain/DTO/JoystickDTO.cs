using MichaelTCC.Infrastructure.DTO;

namespace MichaelTCC.Domain.DTO
{
    class JoystickDTO : IJoystickDTO
    {
        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool iOS { get; set; }
        public bool X { get; set; }
        public bool A { get; set; }
        public bool Triangle { get; set; }
    }
}