using System;
using MichaelTCC.Infrastructure.DTO;

namespace MichaelTCC.Domain.DTO
{
    public class VideoConfigurationDTO : IVideoConfigurationDTO
    {
        public string Url { get; set; }
    }
}