using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus
{
    public sealed class SerialTransport : NibusCodec<byte[], byte[]>, INibusEndpoint<byte[]>
    {
        private readonly SerialPort _serial;
        private readonly CancellationTokenSource _cts;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public SerialTransport(string portName, int baudRate)
        {
            Contract.Ensures(Decoder != null);
            Contract.Ensures(Encoder != null);
            _serial = new SerialPort(portName, baudRate) { DtrEnable = true };
            
            _cts = new CancellationTokenSource();
            var options = new DataflowBlockOptions { CancellationToken = _cts.Token };
            Encoder = new BufferBlock<byte[]>(options);
            Decoder = new BufferBlock<byte[]>(options);

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
                var data = await Encoder.ReceiveAsync(_cts.Token).ConfigureAwait(false);
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
                Decoder.Post(buffer);
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _cts.Cancel();
                _serial.Dispose();
            }
        }
    }
}
