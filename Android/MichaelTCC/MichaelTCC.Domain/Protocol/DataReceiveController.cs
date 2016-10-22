using System.Threading;
using MichaelTCC.Infrastructure.Protocol;
using Newtonsoft.Json;

namespace MichaelTCC.Domain.Protocol
{
    public class DataReceiveController
    {
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private IDataReceiveProtocol _protocol = new DataReceiveProtocol { Campo1 = "Ola", Campo2 = "Teste", Campo3 = "3", Campo4 = "4", Campo5 = "5" };

        internal void SetDataProtocol(IDataReceiveProtocol dataProtocol)
        {
            _semaphore.Wait();
            try
            {
                _protocol = dataProtocol;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public string GetJsonDataProtocol()
        {
            _semaphore.Wait();
            try
            {
                return JsonConvert.SerializeObject(_protocol as DataReceiveProtocol);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}