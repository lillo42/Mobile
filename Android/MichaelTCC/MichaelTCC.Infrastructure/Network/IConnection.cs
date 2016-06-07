using System;
using System.Threading.Tasks;

namespace MichaelTCC.Infrastructure.Network
{
    public interface IConnection
    {
        event EventHandler<byte[]> OnDataReceive;
        event EventHandler<string> OnError;

        void SendData(byte[] data);

        bool IsListening { get; }

        void StartListening();
        void StopListening();
    }
}