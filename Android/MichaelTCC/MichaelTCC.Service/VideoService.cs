using MichaelTCC.Infrastructure.DTO;
using System.Threading.Tasks;

namespace MichaelTCC.Service
{
    public class VideoService : BaseService
    {
        private readonly ConfigurationService _configurationService;

        public VideoService(ConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public IVideoConfigurationDTO  GetVideoConfiguration()
        {
            return _configurationService.ReadVideo();
        }
    }
}