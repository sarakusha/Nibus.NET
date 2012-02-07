using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus
{
    /// <summary>
    /// Класс реализует транспорт для NiBUS на уровне последовательного COM-протокола.
    /// </summary>
    public sealed class SerialTransport : INibusEndpoint<byte[]>
    {
        private readonly SerialPort _serial;
        private readonly CancellationTokenSource _cts;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly BufferBlock<byte[]> _incomingMessages;

        private readonly BufferBlock<byte[]> _outgoingMessages;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialTransport"/> class.
        /// </summary>
        /// <param name="portName">Имя последовательного COM-порта.</param>
        /// <param name="baudRate">Скорость порта (115200/28800).</param>
        /// <param name="immediatelyOpen">Если <c>true</c> то сразу открыть COM-порт.</param>
        public SerialTransport(string portName, int baudRate, bool immediatelyOpen = false)
        {
            Contract.Ensures(OutgoingMessages != null);
            Contract.Ensures(IncomingMessages != null);

            _cts = new CancellationTokenSource();
            var options = new DataflowBlockOptions {CancellationToken = _cts.Token};
            _incomingMessages = new BufferBlock<byte[]>(options);
            _outgoingMessages = new BufferBlock<byte[]>(options);

            _serial = new SerialPort(portName, baudRate) { DtrEnable = true };
            _serial.DataReceived += SerialDataReceived;
            if (immediatelyOpen)
            {
                Open();
            }
        }

        /// <summary>
        /// Открыть COM-порт.
        /// </summary>
        public void Open()
        {
            _serial.Open();
            RunAsyncInternal();
        }

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

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
            }

            if (_serial != null)
            {
                _serial.Dispose();
            }
        }

        #endregion

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

        private async Task Consumer()
        {
            while (_serial.IsOpen && !_cts.IsCancellationRequested)
            {
                var data = await _outgoingMessages.ReceiveAsync(_cts.Token).ConfigureAwait(false);
                if (data.Length > 0)
                {
                    await WriteAsync(data).ConfigureAwait(false);
                    if (Logger.IsTraceEnabled)
                    {
                        Logger.Trace(String.Join(":", data.Select(b => b.ToString("X2")).ToArray()));
                    }
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
                _incomingMessages.Post(buffer);
                if (Logger.IsTraceEnabled)
                {
                    Logger.Trace(String.Join(":", buffer.Select(b => b.ToString("X2")).ToArray()));
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error while reading", ex);
            }
        }

        #region Implementation of ICodecInfo

        /// <summary>
        /// Возвращает описание кодека.
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}