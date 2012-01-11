//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsStartProgramInvocation.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

#endregion

namespace NataInfo.Nibus.Nms
{
    internal sealed class NmsStartProgramInvocation : NmsMessage
    {
        #region Member Variables

        private ReadOnlyCollection<Tuple<NmsValueType, object>> _args;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsStartProgramInvocation(NibusDatagram datagram) : base(datagram)
        {
            Contract.Assume(ServiceType == NmsServiceType.StartProgramInvocation);
        }

        public NmsStartProgramInvocation(
            Address source, Address destanation, int id, params Tuple<NmsValueType, object>[] args)
        {
            Contract.Ensures(ServiceType == NmsServiceType.StartProgramInvocation);
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
                NmsServiceType.StartProgramInvocation,
                true,
                id,
                false,
                nmsData.ToArray());
        }

        #endregion //Constructors

        #region Properties

        public ReadOnlyCollection<Tuple<NmsValueType, object>> Args
        {
            get
            {
                Contract.Requires(!IsResponce);
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

        public int ErrorCode
        {
            get
            {
                Contract.Requires(IsResponce);
                return Datagram.Data[NmsHeaderLength + 0];
            }
        }

        #endregion //Properties
    }
}