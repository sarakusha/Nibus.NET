using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus
{
    public sealed class SerialTransport : INibusEndpoint<byte[], byte[]>
    {
        private readonly SerialPort _serial;
        private readonly BufferBlock<byte[]> _incoming;
        private readonly BufferBlock<byte[]> _outgoing;
        private readonly CancellationTokenSource _cts;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public SerialTransport(string portName, int baudRate)
        {
            _serial = new SerialPort(portName, baudRate) {DtrEnable = true};
            _serial.DataReceived += SerialDataReceived;
            _cts = new CancellationTokenSource();
            _incoming = new BufferBlock<byte[]>();
            _outgoing = new BufferBlock<byte[]>();
        }

        #region Implementation of INibusEndpoint<in byte[],out byte[]>

        public ITargetBlock<byte[]> IncomingMessages
        {
            get { return _incoming; }
        }

        public ISourceBlock<byte[]> OutgoingMessages
        {
            get { return _outgoing; }
        }

        #endregion

        public void RunAsync()
        {
            _serial.Open();
            RunAsyncInternal();
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _cts.Cancel();
            _serial.Dispose();
            Logger.Debug("Disposed.");
        }

        #endregion

        private async Task Consumer()
        {
            while (_serial.IsOpen && !_cts.IsCancellationRequested)
            {
                var data = await _incoming.ReceiveAsync(_cts.Token).ConfigureAwait(false);
                if (data.Length > 0)
                {
                    await WriteAsync(data);
                }
            }
        }

        private Task WriteAsync(byte[] data)
        {
            var stream = _serial.BaseStream;
            return Task.Factory.FromAsync(stream.BeginWrite, stream.EndWrite, data, 0, data.Length, null);
        }

        private void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var buffer = new byte[_serial.BytesToRead];
                _serial.Read(buffer, 0, buffer.Length);
                _outgoing.Post(buffer);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error while reading", ex);
            }
        }

        private async void RunAsyncInternal()
        {
            await Consumer();
        }
    }
}
