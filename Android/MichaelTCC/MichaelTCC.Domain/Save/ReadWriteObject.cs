using Java.IO;
using MichaelTCC.Domain.DTO;
using MichaelTCC.Infrastructure.DTO;
using MichaelTCC.Infrastructure.Files;
using Org.Json;

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
            json.Put(nameof(ITcpConfigurationDTO.Time), tcp.Time);
            TextFile.Save(filesDir, c_tcpJson, json.ToString());
            Core.Instance.UpdateTcpConfiguration(tcp);
        }

        public static IVideoConfigurationDTO ReadVideo(File filesDir)
        {
            IVideoConfigurationDTO video = new VideoConfigurationDTO();
            try
            {
                string file = TextFile.Read(filesDir, c_tcpJson);
                if (!string.IsNullOrEmpty(file))
                {
                    var json = new JSONObject(TextFile.Read(filesDir, c_urlJson));
                    video = new VideoConfigurationDTO();
                    video.Url = json.GetString(nameof(IVideoConfigurationDTO.Url));
                }
                return video;
            }
            catch
            {
                return video;
            }
        }

        public static ITcpConfigurationDTO ReadTcp(File filesDir)
        {
            ITcpConfigurationDTO tcp = new TcpConfigurationDTO { Port = 8000, Time = 100 };
            try
            {
                string file = TextFile.Read(filesDir, c_tcpJson);
                if (!string.IsNullOrEmpty(file))
                {
                    var json = new JSONObject(TextFile.Read(filesDir, c_tcpJson));
                    tcp = new TcpConfigurationDTO();
                    tcp.Port = json.GetInt(nameof(ITcpConfigurationDTO.Port));
                    tcp.Time = json.GetInt(nameof(ITcpConfigurationDTO.Time));
                }

                return tcp;
            }
            catch
            {
                return tcp;
            }
        }
    }
}