using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus
{
    public sealed class SerialTransport : INibusEndpoint<byte[]>, IDisposable
    {
        private readonly SerialPort _serial;
        private readonly CancellationTokenSource _cts;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly BufferBlock<byte[]> _incomingMessages;

        private readonly BufferBlock<byte[]> _outgoingMessages;

        public SerialTransport(string portName, int baudRate)
        {
            Contract.Ensures(OutgoingMessages != null);
            Contract.Ensures(IncomingMessages != null);
            _serial = new SerialPort(portName, baudRate) { DtrEnable = true };
            
            _cts = new CancellationTokenSource();
            var options = new DataflowBlockOptions { CancellationToken = _cts.Token };
            _incomingMessages = new BufferBlock<byte[]>(options);
            _outgoingMessages = new BufferBlock<byte[]>(options);

            _serial.DataReceived += SerialDataReceived;
        }

        public void RunAsync()
        {
            _serial.Open();
            RunAsyncInternal();
        }

        private async Task Consumer()
        {
            while (_serial.IsOpen && !_cts.IsCancellationRequested)
            {
                var data = await _outgoingMessages.ReceiveAsync(_cts.Token).ConfigureAwait(false);
                if (data.Length > 0)
                {
                    await WriteAsync(data).ConfigureAwait(false);
                }
            }
        }

        private Task WriteAsync(byte[] data)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug(String.Join(":", data.Select(b => b.ToString("X2")).ToArray()));
            }
            var stream = _serial.BaseStream;
            return Task.Factory.FromAsync(stream.BeginWrite, stream.EndWrite, data, 0, data.Length, null);
        }

        private void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var buffer = new byte[_serial.BytesToRead];
                _serial.Read(buffer, 0, buffer.Length);
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug(String.Join(":", buffer.Select(b => b.ToString("X2")).ToArray()));
                }

                _incomingMessages.Post(buffer);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error while reading", ex);
            }
        }

        private async void RunAsyncInternal()
        {
            try
            {
                await Consumer();
            }
            catch (TaskCanceledException)
            {
            }
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (_cts != null)
            {
                _cts.Cancel();
            }

            if (_serial != null)
            {
                _serial.Dispose();
            }
        }

        #endregion

        #region Implementation of INibusEndpoint<byte[]>

        /// <summary>
        /// Из COM-порта
        /// </summary>
        public IReceivableSourceBlock<byte[]> IncomingMessages
        {
            get { return _incomingMessages; }
        }

        /// <summary>
        /// В COM-порт
        /// </summary>
        public ITargetBlock<byte[]> OutgoingMessages
        {
            get { return _outgoingMessages; }
        }

        #endregion
    }
}
