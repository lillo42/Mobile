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

        internal TcpServerNetwork(int port) : base(port)
        {

        }

        public bool IsListening { get { return Active; } }

        public void SendData(byte[] data)
        {
            if (_tcpClient == null)
                throw new ArgumentNullException("Conection not start");

            NetworkStream network = _tcpClient.GetStream();
            byte[] dataLength = BitConverter.GetBytes(data.Length);

            network.Write(dataLength, 0, dataLength.Length);
            network.Flush();

            network.Write(data, 0, data.Length);
            network.Flush();
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

        private Task Listening(CancellationToken cancel)
        {
            Start();
            return new TaskFactory().StartNew(() =>
            {
                _tcpClient = AcceptTcpClient();
                NetworkStream network = _tcpClient.GetStream();
                while (!cancel.IsCancellationRequested)
                {
                    try
                    {
                        int read = network.ReadByte();

                        OnDataReceive?.Invoke(this, new byte[] { (byte)read });
                    }
                    catch(Exception e)
                    {
                        OnError?.Invoke(this, e.ToString());
                    }
                }
            });
        }

        public void Dispose()
        {
            Dispose(isDisposed);
            isDisposed = false;
        }

        private void Dispose(bool dispose)
        {
            if (dispose)
            {
                if (Active && Server != null && _tcpClient != null)
                {
                    Server.Shutdown(SocketShutdown.Both);
                    Server.Close();
                }
            }
        }
    }
}