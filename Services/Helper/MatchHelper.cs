    using BetDND.Models;
    using BetDND.Data;
    using System.Linq;
    using BetDND.Enums;

    namespace BetDND.Services
    {
        public class MatchHelperService
        {
            private readonly DataContext _context;
            private readonly SubBetService subBetService;
            
            public MatchHelperService(DataContext context)
            {
                _context = context;
                subBetService = new SubBetService(_context);
            }

            public List<Match> GetUnfinishedMatches()
            {
                var matches = _context.Matches.Where(m => m.IsFinished == false).ToList();
                return matches;
            }

            public object CreateMatchObject(Match match)
            {
                var id = match.Id;
                string sport = GetSportName(match.Sport);
                string country = match.Country;
                string countryCode = match.CountryCode;
                string league = match.League;
                string date = match.Date;
                string time = match.Time;
                string minWithMacth = GetMbsName(match.Mbs);
                string homeTeam = match.HomeTeam;
                string awayTeam = match.AwayTeam;
                var mainBetOptions = GetMainBetOptions(id);
                var subBetOptions = subBetService.GetSubBetOptions(id);
                var matchObject = new
                {
                    id = id,
                    sport = sport,
                    country = country,
                    countryCode = countryCode,
                    league = league,
                    date = date,
                    time = time,
                    minWithMacth = minWithMacth,
                    homeTeam = homeTeam,
                    awayTeam = awayTeam,
                    mainBetOption = mainBetOptions,
                    subBetOptions = subBetOptions
                };
                return matchObject;
            }

            public Dictionary<string,double> GetMainBetOptions(int matchId)
            {
                var mainBetOption = _context.MainBetOptions.FirstOrDefault(m => m.MatchId == matchId);
                if (mainBetOption == null)
                {
                    return new Dictionary<string, double>();
                }
                return new Dictionary<string, double>
                {
                    { "1", mainBetOption.HomeOdd },
                    { "X", mainBetOption.DrawOdd },
                    { "2", mainBetOption.AwayOdd }
                };
            }

            public string GetSportName(MatchSports sport)
            {
                switch(sport)
                {
                    case MatchSports.Basketball:
                        return "Basketbol";
                    case MatchSports.Football:
                        return "Futbol";
                    case MatchSports.IceHockey:
                        return "Buz Hokeyi";
                    case MatchSports.Tennis:
                        return "Tenis";
                    case MatchSports.Handball:
                        return "Hentbol";
                    default:
                        return null;
                }
            }

            public string GetMbsName(MBSRule mbs)
            {
                switch(mbs)
                {
                    case MBSRule.One:
                        return "1";
                    case MBSRule.Two:
                        return "2";
                    case MBSRule.Three:
                        return "3";
                    default:
                        return null;
                }
            }

            public Match GetMatchById(int matchId)
            {
                return _context.Matches.FirstOrDefault(m => m.Id == matchId);
            }

            public MainBetOptions GetMainBetOptionByMatchId(int matchId)
            {
                return _context.MainBetOptions.FirstOrDefault(m => m.MatchId == matchId);
            }

            
        }
    }