//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsExecuteProgramInvocation.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

#endregion

namespace NataInfo.Nibus.Nms.Services
{
    public sealed class NmsExecuteProgramInvocation : NmsMessage
    {
        #region Member Variables

        private ReadOnlyCollection<Tuple<NmsValueType, object>> _args;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        internal NmsExecuteProgramInvocation(NibusDatagram datagram)
            : base(datagram)
        {
            Contract.Requires(datagram != null);
            Contract.Requires(datagram.ProtocolType == ProtocolType.Nms);
            Contract.Requires(datagram.Data.Count >= NmsHeaderLength);
            Contract.Assume(ServiceType == NmsServiceType.ExecuteProgramInvocation);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NmsExecuteProgramInvocation"/> class.
        /// </summary>
        /// <param name="source">Адрес источника.</param>
        /// <param name="destanation">Адрес приемника.</param>
        /// <param name="id">The id.</param>
        /// <param name="waitResponse"><c>true</c> - ожидать подтверждения, иначе - <c>false</c>.</param>
        /// <param name="args">Аргументы, передаваемые в подпрограмму в виде массива пар,
        /// где первый элемент пары содержит тип параметра, второй - значение.</param>
        public NmsExecuteProgramInvocation(
            Address source, Address destanation, int id, bool waitResponse = true, params Tuple<NmsValueType, object>[] args)
        {
            Contract.Ensures(ServiceType == NmsServiceType.ExecuteProgramInvocation);
            var nmsData = new List<byte>(NmsMaxDataLength) { (byte)args.Length };
            foreach (var argData in args.Select(tuple => WriteValue(tuple.Item1, tuple.Item2)))
            {
                if (nmsData.Count + argData.Length > NmsMaxDataLength)
                {
                    throw new ArgumentOutOfRangeException("args");
                }

                nmsData.AddRange(argData);
            }

            Initialize(
                source,
                destanation,
                PriorityType.Normal,
                NmsServiceType.ExecuteProgramInvocation,
                waitResponse,
                id,
                false,
                nmsData.ToArray());
        }

        #endregion //Constructors

        #region Properties

        /// <summary>
        /// Возвращает аргументы, передаваемые в подпрограмму.
        /// </summary>
        public ReadOnlyCollection<Tuple<NmsValueType, object>> Args
        {
            get
            {
                Contract.Requires(!IsResponse);
                if (_args == null)
                {
                    byte count = Datagram.Data[NmsHeaderLength + 0];
                    var args = new Tuple<NmsValueType, object>[count];
                    byte[] buffer = Datagram.Data.Skip(1).ToArray();
                    int offset = 0;
                    for (int i = 0; i < count; i++)
                    {
                        var vt = (NmsValueType)buffer[offset++];
                        object value = ReadValue(vt, buffer, offset);
                        offset += GetSizeOf(vt, value);
                        args[i] = new Tuple<NmsValueType, object>(vt, value);
                    }
                    _args = new ReadOnlyCollection<Tuple<NmsValueType, object>>(args);
                }

                return _args;
            }
        }

        #endregion //Properties
    }
}