//-------------------------------------------------------------------
// Copyright (c) 2012 Nata-Info Ltd.
// Andrei Sarakeev
// NmsInformationReport.cs
// 
//-------------------------------------------------------------------

#region Using directives

using System.Diagnostics.Contracts;
using System.Linq;

#endregion

namespace NataInfo.Nibus.Nms
{
    public sealed class NmsInformationReport : NmsMessage
    {
        #region Member Variables

        private object _value;

        #endregion

        public enum SportReports : byte
        {
            Timer = 5,
            HomeTeamScore = 6,
            VisitingTeamScore = 7,
            Period = 8,
            HomeTeamFoul = 9,
            VisitingTeamFoul = 10,
            HomeTeamTimebreaks = 14,
            VisitingTeamTimebreaks = 15,
            PlayerCount = 16,
            PlayerInfo = 17,
            PlayerStat = 18,
            HomeTeamName = 19,
            VisitingTeamName = 20,
            HomeTeamCountry = 21,
            VisitingTeamCountry = 22,
            TournamentName = 23,
            BallOwner = 24,
            ShowMessage = 25,
            ChangeSport = 27
        }

        #region Constructors

        /// <summary>
        /// The default Constructor.
        /// </summary>
        public NmsInformationReport(NibusDatagram datagram) : base(datagram)
        {
            Contract.Ensures(!IsResponce);
            Contract.Assume(ServiceType == NmsServiceType.InformationReport);
        }

        public NmsInformationReport(Address source, int id, NmsValueType valueType, object value, PriorityType priority = PriorityType.Normal)
        {
            Contract.Ensures(!IsResponce);
            Contract.Ensures(ServiceType == NmsServiceType.InformationReport);
            Initialize(
                source,
                Address.Broadcast,
                priority,
                NmsServiceType.InformationReport,
                false,
                id,
                false,
                WriteValue(valueType, value));
        }

        #endregion //Constructors

        #region Properties

        public NmsValueType ValueType
        {
            get { return (NmsValueType)Datagram.Data[0]; }
        }

        public object Value
        {
            get { return _value ?? (_value = ReadValue(ValueType, Datagram.Data.ToArray(), 1)); }
        }

        #endregion //Properties
    }
}