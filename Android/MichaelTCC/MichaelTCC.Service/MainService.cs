using Java.IO;
using MichaelTCC.Domain;
using MichaelTCC.Domain.Protocol;
using MichaelTCC.Infrastructure.DTO;

namespace MichaelTCC.Service
{
    public sealed class MainService : BaseService
    {
        private readonly NetworkService _networkService;
        private readonly SensorService _sensorService;
        private readonly ConfigurationService _configService;
        private readonly MichaelProtocolBuilder _builder = new MichaelProtocolBuilder();

        public MainService(NetworkService networkService, SensorService sensorService, ConfigurationService configurationService)
        {
            _networkService = networkService;
            _sensorService = sensorService;
            _configService = configurationService;

            _networkService.OnNotifition += NetworkService_OnNotifition;
        }

        public void StartServer()
        {
            Core.Instance.SensorCapture = _sensorService.SensorCapture;
            Core.Instance.StartServerTcp(_configService.ReadTcp());
        }

        private void ConfigService_OnTcpConfigUpdate(object sender, ITcpConfigurationDTO e)
        {
            _networkService.CreateTcpConnection(e);
        }

        private void NetworkService_OnNotifition(object sender, string e)
        {
            InvokeOnNotifition(e);
        }
    }
}