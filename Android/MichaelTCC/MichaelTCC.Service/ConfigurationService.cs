using Java.IO;
using MichaelTCC.Infrastructure.DTO;
using MichaelTCC.Domain.Save;

namespace MichaelTCC.Service
{
    public sealed class ConfigurationService : BaseService
    {
        private readonly File _fileDir;

        public ConfigurationService(File fileDir)
        {
            _fileDir = fileDir;
        }

        public void Save(ITcpConfigurationDTO tcpDTO)
        {
            ReadWriteObject.Save(_fileDir, tcpDTO);
        }

        public void Save(IVideoConfigurationDTO videoDTO)
        {
            ReadWriteObject.Save(_fileDir, videoDTO);
        }

        public ITcpConfigurationDTO ReadTcp()
        {
            return ReadWriteObject.ReadTcp(_fileDir);
        }

        public IVideoConfigurationDTO ReadVideo()
        {
            return ReadWriteObject.ReadVideo(_fileDir);
        }
    }
}