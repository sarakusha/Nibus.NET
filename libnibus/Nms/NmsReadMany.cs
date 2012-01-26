//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsReadMany.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

#endregion

namespace NataInfo.Nibus.Nms
{
    public sealed class NmsReadMany : NmsMessage
    {
        #region Member Variables

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsReadMany(Address source, Address destanation, params int[] ids)
        {
            Contract.Requires(0 < ids.Length);
            Contract.Requires(ids.Length <= MaxReadVariables);
            var data = ids.SelectMany(
                id => new[]
                          {
                              (byte)(((byte)NmsServiceType.Read << 3) | (id >> 8)),
                              (byte)(id & 0xFF),
                              (byte)0
                          }).ToArray();
            Datagram = new NibusDatagram(null, source, destanation, PriorityType.Normal, ProtocolType.Nms, data);
        }

        public NmsReadMany(Address destanation, params int[] ids) : this(Address.Empty, destanation, ids)
        {
        }

        #endregion //Constructors

        #region Properties

        #endregion //Properties

        #region Methods

        #endregion //Methods

        #region Implementations

        #endregion //Implementations
    }
}
