//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// TennisReceiver.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NataInfo.Nibus.Nms;

#endregion

namespace NataInfo.Nibus.Sport
{
    public class TennisReceiver : GameReceiver
    {
        #region Member Variables

        public const int TennisStats = 32;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public TennisReceiver(NmsController nmsController) : base(nmsController)
        {
        }

        #endregion //Constructors

        #region Events

        public event EventHandler<TennisStatChangedEventArgs> TennisStatChanged;
        #endregion


        #region Properties

        #endregion //Properties

        #region Methods

        #endregion //Methods

        #region Implementations

        protected override void OnInformationReportReceived(object sender, NmsInformationReportEventArgs e)
        {
            base.OnInformationReportReceived(sender, e);
            if (e.Identified)
            {
                return;
            }

            if (e.InformationReport.Id == TennisStats)
            {
                var report = e.InformationReport;
                var source = report.Datagram.Source;
                SafeInvokeEvent(TennisStatChanged, () => new TennisStatChangedEventArgs(source, report.GetTennisStat()));
                e.Identified = true;
            }
        }
        #endregion //Implementations
    }
}
