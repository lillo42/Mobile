using System;
using System.Linq;
using Android.Bluetooth;
using Java.Util;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace MichaelTCC.Infrastructure.Network
{
    public class BluetoothConnection: IConnection
    {
        internal BluetoothConnection(string deviceName, UUID uuid)
        {
            _deviceName = deviceName;
            _uuid = uuid;
        }

        private readonly string _deviceName;
        private readonly UUID _uuid;

        public event EventHandler<byte[]> OnDataReceive;
        public event EventHandler<string> OnError;

        private BluetoothAdapter _bluetoothAdapter;
        private BluetoothDevice _device;
        private BluetoothSocket _socket;
        //private static UUID JOYSTICK_UUID = UUID.FromString("00001124-0000-1000-8000-00805f9b34fb");
        private Task _taskRead;
        private CancellationTokenSource _cancel;

        public bool IsListening
        {
            get
            {
                return _socket != null && _socket.IsConnected;
            }
        }

        public void StartListening()
        {
            if (!CheckBluetooh())
                return;
            if (_cancel != null)
                StopListening();
            _cancel = new CancellationTokenSource();

            _device = _bluetoothAdapter.BondedDevices.FirstOrDefault(x => x.Name == _deviceName);
            if (_device != null)
            {
                try
                {
                    _socket = _device.CreateInsecureRfcommSocketToServiceRecord(_uuid);
                    _socket.Connect();
                }
                catch(Exception e)
                {
                    OnError?.Invoke(this, e.ToString());
                }
            }
        }

        private bool CheckBluetooh()
        {
            bool returing = false;
            _bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (_bluetoothAdapter != null)
            {
                returing = false;
                if (!_bluetoothAdapter.IsEnabled)
                    _bluetoothAdapter.Enable();
            }

            return returing;
        }

        public void StopListening()
        {
            if (_cancel != null)
                _cancel.Cancel();
            Task.Delay(10).Wait();

            if (_socket != null && _socket.IsConnected)
                _socket.Close();
        }

        private Task Read()
        {
            _cancel = new CancellationTokenSource();
            return ReadAsync(_cancel.Token);
        }

        public async Task SendDataAsync(byte[] data)
        {
            Stream outStream = _socket.OutputStream;

            byte[] dataLength = BitConverter.GetBytes(data.Length);

            await outStream.WriteAsync(dataLength, 0, dataLength.Length);
            await outStream.FlushAsync();

            await outStream.WriteAsync(data, 0, data.Length);
            await outStream.FlushAsync();
        }

        private async Task ReadAsync(CancellationToken token)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytes;

                Stream inputStream = _socket.InputStream;

                while (!_cancel.IsCancellationRequested)
                {
                    bytes = await inputStream.ReadAsync(buffer, 0, buffer.Length, token);
                    OnDataReceive?.Invoke(this, buffer);
                }

                inputStream.Close();
            }
            catch(Exception e)
            {
                OnError?.Invoke(this, e.ToString());
            }
        }
    }
}