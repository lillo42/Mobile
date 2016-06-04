using System;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace MichaelTCC.Infrastructure.Network
{
    public class TcpServerNetwork : TcpListener, IConnection, IDisposable
    {

        public event EventHandler<byte[]> OnDataReceive;
        public event EventHandler<string> OnError;

        private TcpClient _tcpClient;
        private CancellationTokenSource _cancel;
        private Task _listening;
        private bool isDisposed = true;

        internal TcpServerNetwork(IPAddress localaddr, int port):base(localaddr,port)
        {

        }

        public bool IsListening { get { return Active; } }

        public async Task SendDataAsync(byte[] data)
        {
            if (_tcpClient == null)
                throw new ArgumentNullException("Conection not start");

            NetworkStream network = _tcpClient.GetStream();
            byte[] dataLength = BitConverter.GetBytes(data.Length);

            await network.WriteAsync(dataLength, 0, dataLength.Length);
            await network.FlushAsync();

            await network.WriteAsync(data, 0, data.Length);
            await network.FlushAsync();
        }

        public void StartListening()
        {
            if (_cancel != null)
                StopListening();
            _cancel = new CancellationTokenSource();
            _listening = Listening(_cancel.Token);
        }

        public void StopListening()
        {
            _cancel.Cancel();
            Task.Delay(10).Wait();
            _cancel.Dispose();
        }

        private async Task Listening(CancellationToken cancel)
        {
            _tcpClient = await AcceptTcpClientAsync();
            NetworkStream network = _tcpClient.GetStream();
            while (!cancel.IsCancellationRequested)
            {
                var lenghtByte = new byte[sizeof(int)];
                int receive = await network.ReadAsync(lenghtByte, 0, lenghtByte.Length, cancel);
                //Receive stop
                if (receive != sizeof(int) || cancel.IsCancellationRequested)
                    break;

                int lenghtData = BitConverter.ToInt32(lenghtByte, 0);
                var dataRecive = new byte[lenghtData];
                receive = await network.ReadAsync(dataRecive, 0, dataRecive.Length, cancel);

                if (receive != lenghtData || cancel.IsCancellationRequested)
                    break;

                OnDataReceive?.Invoke(this, dataRecive);
            }
        }

        public void Dispose()
        {
            Dispose(isDisposed);
            isDisposed = false;
        }

        private void Dispose(bool dispose)
        {
            if(dispose)
            {
                Server.Shutdown(SocketShutdown.Both);
                Server.Close();
            }
        }
    }
}