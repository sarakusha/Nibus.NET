//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NibusStack.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using NataInfo.Nibus.Nms;

#endregion

namespace NataInfo.Nibus
{
    /// <summary>
    /// Стек NiBUS.
    /// </summary>
    public class NibusStack : IDisposable
    {
        #region Member Variables

        private readonly HashSet<ICodecInfo> _codecs;

        #endregion

        /// <summary>
        /// Создает стек на основе последовательного порта, содержащий <see cref="NmsCodec"/>
        /// </summary>
        /// <param name="portName">Имя последовательного COM-порта.</param>
        /// <param name="baudRate">Скорость порта (115200/28800).</param>
        /// <returns>NiBUS стек.</returns>
        public static NibusStack CreateSerialNmsStack(string portName, int baudRate = 115200)
        {
            return new NibusStack(new SerialTransport(portName, baudRate, true), new NibusDataCodec(), new NmsCodec());
        }

        #region Constructors

        /// <summary>
        /// Создает экземпляр <see cref="NibusStack"/> и связывает переданные кодеки <paramref name="codecs"/>.
        /// </summary>
        /// <param name="codecs">Список кодеков в порядке от нижнего к высшему.</param>
        /// <example>
        /// <code>
        /// using (stack = new NibusStack(SerialTransport("COM3", 115200), NibusDataCodec(), NmsCodec())
        /// {
        ///     var nmsProtocol = _stack.GetCodec&lt;NmsCodec&gt;().Protocol;
        /// }
        /// </code>
        /// </example>
        public NibusStack(params ICodecInfo[] codecs)
        {
            _codecs = new HashSet<ICodecInfo>();
            if (codecs.Length > 0)
            {
                AddChain(codecs);
            }
        }

        #endregion //Constructors

        #region Properties

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Создает связь между кодеками. Кодеки могут быть как новыми (тогда они добавляются в стек), так и существующими. 
        /// </summary>
        /// <param name="codecs">Кодеки.</param>
        public void AddChain(params ICodecInfo[] codecs)
        {
            Contract.Requires(codecs.Length >= 2);
            if (codecs.Length < 2)
            {
                throw new ArgumentException("Must be at least two codecs");
            }

            var links = new List<IDisposable>(codecs.Length - 1);
            for (var i = codecs.Length - 1; i > 0; i--)
            {
                var link = DynamicConnectTo(codecs[i], codecs[i - 1]);
                if (link == null)
                {
                    links.ForEach(l => l.Dispose());
                    throw new ArgumentException("Invalid codec type");
                }

                links.Add(link);
            }

            codecs.ToList().ForEach(codec => _codecs.Add(codec));
        }

        /// <summary>
        /// Возвращает все кодеки указанного типа.
        /// </summary>
        /// <typeparam name="T">Тип кодека.</typeparam>
        /// <returns>Кодеки заданного типа присутствующие в стеке.</returns>
        public IEnumerable<T> GetCodecs<T>()
        {
            return _codecs.OfType<T>();
        }

        /// <summary>
        /// Возвращает единственный кодек указанного типа. Вызывает исключение, если кодеков несколько или отсутствует.
        /// </summary>
        /// <typeparam name="T">Тип кодека.</typeparam>
        /// <returns>Кодек указанного типа.</returns>
        public T GetCodec<T>()
        {
            return _codecs.OfType<T>().Single();
        }

        #endregion //Methods

        #region Implementations

        private static IDisposable DynamicConnectTo(object topCodec, object bottomCodec)
        {
            var bottomCodecType = bottomCodec.GetType();
            foreach (MethodInfo mi in topCodec.GetType().GetMember(
                        "ConnectTo", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public))
            {
                var pi = mi.GetParameters().Single();
                var it = bottomCodecType.GetInterface(pi.ParameterType.Name);
                if (it != null)
                {
                    var connectTo = mi;
                    if (mi.IsGenericMethod)
                    {
                        var genTypes = it.GetGenericArguments();
                        connectTo = mi.MakeGenericMethod(genTypes.First());
                    }

                    return (IDisposable)connectTo.Invoke(topCodec, new[] { bottomCodec });
                }
            }

            return null;
        }

        #endregion //Implementations

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _codecs.Cast<IDisposable>().ToList().ForEach(codec => codec.Dispose());
        }

        #endregion
    }
}