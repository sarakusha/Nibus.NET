using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus
{
    public class NmsProtocol : INibusProtocol
    {
        private readonly CancellationTokenSource _cts;
        private readonly TransformBlock<NibusDatagram, NmsMessage> _decoder;
        private readonly TransformManyBlock<IList<NmsMessage>, NibusDatagram> _encoder;

        private enum ValueType
        {
            Boolean = 11,       // 8 бит Значение TRUE = 1/FALSE = 0
            Int8 = 16,          // 8 бит Знаковый байт
            Int16 = 2,          // 16 бит Знаковое короткое целое
            Int32 = 3,          // 32 бита Знаковое целое
            Int64 = 20,         // 64 бита Знаковое длинное целое
            UInt8 = 17,         // 8 бит Байт
            UInt16 = 18,        // 16 бит Короткое целое
            UInt32 = 19,        // 32 бита Целое
            UInt64 = 21,        // 64 бита Длинное целое
            Real32 = 4,         // 32 бита Значение с плавающей точкой
            Real64 = 5,         // 64 бита Значение с плавающей точкой удвоенной точности
            String = 30,        // Строка символов с терминирующим нулем
            DateTime = 7,       // 80 бит Дата/время в формате BCD
                                // DD-MM-YYYY HH:MM:SS.0mmmbW
                                // DD – дата
                                // MM – месяц
                                // YYYY – год
                                // HH – час (0..23)
                                // MM – минуты
                                // SS – секунды
                                // mmm – миллисекунды
                                // W – день недели (1..7,
                                // 1 – вс,
                                // 2 – пн,
                                // … 7 – сб)
                                // b – зарезервировано
            Array = 0x80
        }

        public NmsProtocol()
        {
            _cts = new CancellationTokenSource();
            var options = new ExecutionDataflowBlockOptions {CancellationToken = _cts.Token};
            _decoder = new TransformBlock<NibusDatagram, NmsMessage>(d => new NmsMessage(d), options);
            _encoder = new TransformManyBlock<IList<NmsMessage>, NibusDatagram>(m => Encode(m), options);
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _cts.Cancel();
        }

        #endregion

        #region Implementation of INibusEndpoint<in NibusDatagram,out NibusDatagram>

        public ITargetBlock<NibusDatagram> IncomingMessages
        {
            get { return _decoder; }
        }

        public ISourceBlock<NibusDatagram> OutgoingMessages
        {
            get { return _encoder; }
        }

        #endregion

        #region Implementation of INibusProtocol

        public ProtocolType Protocol
        {
            get { return ProtocolType.Nms; }
        }

        #endregion

        private static IEnumerable<NibusDatagram> Encode(IList<NmsMessage> nmsMessage)
        {
            var reading = nmsMessage.Where(msg => msg.ServiceType == NmsServiceType.Read)
                .GroupBy(msg => new {msg.Destanation, msg.Source}, msg => msg.Id);
            //test

        }
    }
}
