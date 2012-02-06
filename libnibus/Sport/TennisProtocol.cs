//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// TennisProtocol.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System;
using NataInfo.Nibus.Nms;

#endregion

namespace NataInfo.Nibus.Sport
{
    public sealed class TennisProtocol : GameProtocol
    {
        #region Member Variables

        public const int TennisStats = 32;
        private static ProviderInfo _provider;

        #endregion

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public TennisProtocol(NmsProtocol nmsProtocol) : base(nmsProtocol)
        {
        }

        #endregion //Constructors

        #region Events

        public event EventHandler<TennisStatChangedEventArgs> TennisStatChanged;

        #endregion

        #region Properties

        public override ProviderInfo Provider
        {
            get
            {
                return _provider ??
                       (_provider =
                        new ProviderInfo(
                            (ushort)Providers.Tennis,
                            new TimerAttributes(
                                1, TimerAttributes.Infinity, true, false, false, false, true, false, false, false, "Время игры")));
            }
        }

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
                LazyInvokeEvent(TennisStatChanged, () => new TennisStatChangedEventArgs(source, report.GetTennisStat()));
                e.Identified = true;
            }
        }

        #endregion //Implementations
    }
}