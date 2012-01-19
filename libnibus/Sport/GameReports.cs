namespace NataInfo.Nibus.Sport
{
    public enum GameReports : byte
    {
        Timer = 5,
        HomeTeamScore = 6,
        VisitingTeamScore = 7,
        Period = 8,
        HomeTeamFoul = 9,
        VisitingTeamFoul = 10,
        HomeTeamTimebreaks = 14,
        VisitingTeamTimebreaks = 15,
        TeamCount = 16,
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

    public enum TeamRole : byte
    {
        Home = 0,
        Visiting = 1,
        Judge = 2
    }

    public enum PlayerFunction : byte
    {
        Team = 0,
        Reserve = 1,
        Captain = 2,
        Coach = 3
    }

    public enum BallOwner : byte
    {
        Hidden = 0,
        Home = 1,
        Visiting = 2
    }
}