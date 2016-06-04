using Java.IO;
using MichaelTCC.Domain.DTO;
using MichaelTCC.Infrastructure.DTO;
using MichaelTCC.Infrastructure.Files;
using Org.Json;
using System.Threading.Tasks;

namespace MichaelTCC.Domain.Save
{
    public class ReadWriteObject
    {
        private const string c_urlJson = "url.json";
        private const string c_tcpJson = "tcp.json";

        public static void Save(File filesDir,IVideoConfigurationDTO video)
        {
            var json = new JSONObject();
            json.Put(nameof(IVideoConfigurationDTO.Url), video.Url);
            TextFile.Save(filesDir, c_urlJson, json.ToString());
        }

        public static void Save(File filesDir,ITcpConfigurationDTO tcp)
        {
            var json = new JSONObject();
            json.Put(nameof(ITcpConfigurationDTO.Port), tcp.Port);
            TextFile.Save(filesDir, c_tcpJson, json.ToString());
            Core.Instance.UpdateTcpConfiguration(tcp);
        }

        public static IVideoConfigurationDTO ReadVideo(File filesDir)
        {
            try
            {
                var json = new JSONObject(TextFile.Read(filesDir, c_urlJson));
                IVideoConfigurationDTO video = new VideoConfigurationDTO();
                video.Url = json.GetString(nameof(IVideoConfigurationDTO.Url));
                return video;
            }
            catch
            {
                return new VideoConfigurationDTO();
            }
        }

        public static ITcpConfigurationDTO ReadTcp(File filesDir)
        {
            try
            {
                var json = new JSONObject(TextFile.Read(filesDir, c_tcpJson));
                ITcpConfigurationDTO tcp = new TcpConfigurationDTO();
                tcp.Port = json.GetInt(nameof(ITcpConfigurationDTO.Port));
                return tcp;
            }
            catch
            {
                return new TcpConfigurationDTO { Port = 8000 } ;
            }
        }
    }
}