using System;
using System.Threading.Tasks.Dataflow;

namespace NataInfo.Nibus
{
    /// <summary>
    /// Интерфейс "конечной точки" в стеке протоколов NiBUS.
    /// </summary>
    /// <typeparam name="TData">Тип принимаемых данных.</typeparam>
    /// <remarks>
    /// "Конечная точка" в стеке протоколов NiBUS это либо транспорт (нижний уровень),
    /// либо front-end для какого-либо протокола (высший уровень).
    /// </remarks>
    public interface INibusEndpoint<TData> : IDisposable
    {
        /// <summary>
        /// Возвращает канал-источник входящих сообщений.
        /// </summary>
        IReceivableSourceBlock<TData> IncomingMessages { get; }

        /// <summary>
        /// Возвращает канал-приемник для исходящих сообщений.
        /// </summary>
        ITargetBlock<TData> OutgoingMessages { get; }
    }
}