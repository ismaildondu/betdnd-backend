using BetDND.Models;
using BetDND.Data;
using System.Linq;
using BetDND.Enums;

namespace BetDND.Services
{
    public class BetService {
        public readonly DataContext _context;
        public readonly MatchHelperService matchHelperService;
        public readonly SubBetService subBetservice;

        public BetService(DataContext context)
        {
            _context = context;
            matchHelperService = new MatchHelperService(_context);
            subBetservice = new SubBetService(_context);
        }

        public int AddBet(AddBet addBet, User user) {
            var transaction = _context.Database.BeginTransaction();
            try {
                Bet bet = new Bet {
                    UserId = user.Id,
                    Amount = addBet.amount,
                    Status = BetStatus.Pending
                };
                _context.Bets.Add(bet);
                _context.SaveChanges();
                foreach(NewBet newBet in addBet.newBets) {
                    if(newBet.isMain) {
                        MainBetOptionsType mainBetOptionsType = GetMainBetOptionsTypeByNewBet(newBet.optionName);
                        BetDetail betDetail = new BetDetail {
                            BetId = bet.Id,
                            MatchId = newBet.matchId,
                            IsMainBet = true,
                            MainBetOptionsType = mainBetOptionsType,
                            Odd = GetMainBetOdd(mainBetOptionsType, newBet.matchId)
                        };
                        _context.BetDetails.Add(betDetail);
                        _context.SaveChanges();
                    } else {
                        SubBetSelect subBetSelect = _context.SubBetSelects.FirstOrDefault(subBetSelect => subBetSelect.Id == newBet.select);
                        SubBetOption subBetOption = _context.SubBetOptions.FirstOrDefault(subBetOption => subBetOption.Id == subBetSelect.SubBetOptionId);
                        SubBetCategory subBetCategory = _context.SubBetCategories.FirstOrDefault(subBetCategory => subBetCategory.Id == subBetOption.SubBetCategoryId);
                        BetDetail betDetail = new BetDetail {
                            BetId = bet.Id,
                            MatchId = newBet.matchId,
                            IsMainBet = false,
                            SubBetOptionId = subBetOption.Id,
                            SubBetSelectId = subBetSelect.Id,
                            Odd = subBetSelect.Odd
                        };
                        _context.BetDetails.Add(betDetail);
                        _context.SaveChanges();
                    }
                }
                user.Balance -= addBet.amount;
                _context.Users.Update(user);
                _context.SaveChanges();
                transaction.Commit();
                return bet.Id;
            } catch (System.Exception e) {
                transaction.Rollback();
                throw new System.Exception(e.Message);
            }
        }

        public double GetMainBetOdd(MainBetOptionsType mainBetOptionsType, int matchId)
        {
            var mainBetOption = _context.MainBetOptions.FirstOrDefault(mainBetOption => mainBetOption.MatchId == matchId);
            if(mainBetOptionsType == MainBetOptionsType.Home) {
                return mainBetOption.HomeOdd;
            } else if(mainBetOptionsType == MainBetOptionsType.Away) {
                return mainBetOption.AwayOdd;
            } else {
                return mainBetOption.DrawOdd;
            }
        }

        public MainBetOptionsType GetMainBetOptionsTypeByNewBet(string name) {
            switch (name)
            {
                case "1":
                    return MainBetOptionsType.Home;
                case "2":
                    return MainBetOptionsType.Away;
                case "X":
                    return MainBetOptionsType.Draw;
                default:
                    return MainBetOptionsType.Draw;
            }
        }

        public List<Bet> GetBetsByUser(User user)
        {
            return _context.Bets.Where(bet => bet.UserId == user.Id).ToList();
        }

        public List<BetDetail> GetBetDetailsByBet(Bet bet)
        {
            return _context.BetDetails.Where(betDetail => betDetail.BetId == bet.Id).ToList();
        }

        public double GetBetTotalOdd(Bet bet)
        {
            var betDetails = GetBetDetailsByBet(bet);
            double totalOdd = 1;
            foreach (var betDetail in betDetails)
            {
                totalOdd *= betDetail.Odd;
            }
            return totalOdd;
        }

        public List<Match> GetMatchesByBet(Bet bet)
        {
            var betDetails = GetBetDetailsByBet(bet);
            var matches = new List<Match>();
            foreach (var betDetail in betDetails)
            {
                var match = matchHelperService.GetMatchById(betDetail.MatchId);
                matches.Add(match);
            }
            return matches;
        }

        public Bet GetBetByIdAndUser(int betId, User user)
        {
            return _context.Bets.FirstOrDefault(bet => bet.Id == betId && bet.UserId == user.Id);
        }

        public BetStatus GetBetStatus(Bet bet)
        {
            var betDetails = GetBetDetailsByBet(bet);
            int wonCount = 0;
            int lostCount = 0;
            int pendingCount = 0;
            foreach (var betDetail in betDetails)
            {
                var match = matchHelperService.GetMatchById(betDetail.MatchId);
                var betStatus = GetBetStatusByMatch(match, betDetail);

                if (betStatus == BetStatus.Won)
                {
                    wonCount++;
                }
                else if (betStatus == BetStatus.Lost)
                {
                    lostCount++;
                }
                else
                {
                    pendingCount++;
                }
            }

            if (lostCount > 0)
            {
                return BetStatus.Lost;
            }
            else if (betDetails.Count == wonCount)
            {
                return BetStatus.Won;
            }
            else {
                return BetStatus.Pending;
            }
        }

        public BetStatus GetBetStatusByMatch(Match match,BetDetail betDetail)
        {
            if (!match.IsFinished)
            {
                return BetStatus.Pending;
            }
            if (betDetail.IsMainBet)
            {
                var userMainBetOption = betDetail.MainBetOptionsType ?? MainBetOptionsType.Draw;
                return GetBetStatusByMainBetOption(match, userMainBetOption);
            }
            else
            {
                int subBetOptionId = betDetail.SubBetOptionId ?? 0;
                int subBetSelectId = betDetail.SubBetSelectId ?? 0;
                return GetBetStatusBySubBetOption(subBetOptionId, subBetSelectId);
            }
        }

        public BetStatus GetMatchStatusByBet(Match match, Bet bet) {
            var betDetails = GetBetDetailsByBet(bet);
            var betDetail = betDetails.FirstOrDefault(betDetail => betDetail.MatchId == match.Id);
            return GetBetStatusByMatch(match, betDetail);
        }

        public string BetStatusToString(BetStatus betStatus)
        {
            switch (betStatus)
            {
                case BetStatus.Won:
                    return "Kazandı";
                case BetStatus.Lost:
                    return "Kaybetti";
                case BetStatus.Pending:
                    return "Beklemede";
                default:
                    return "Beklemede";
            }
        }

        public BetStatus GetBetStatusByMainBetOption(Match match, MainBetOptionsType mainBetOptionsType)
        {
            var mainBetOption = matchHelperService.GetMainBetOptionByMatchId(match.Id);
            var mainBetWinner = mainBetOption.MainBetWinner;
            if (MainBetWinnerCheck(mainBetWinner, mainBetOptionsType))
            {
                return BetStatus.Won;
            }
            else
            {
                return BetStatus.Lost;
            }
        }

        public BetStatus GetBetStatusBySubBetOption(int subBetOptionId, int subBetSelectId)
        {
            SubBetOption subBetOption = subBetservice.GetSubBetOptionById(subBetOptionId);
            if (subBetOption.SubBetSelectWinnerId == subBetSelectId)
            {
                return BetStatus.Won;
            }
            else
            {
                return BetStatus.Lost;
            }
        }

        private bool MainBetWinnerCheck(MainBetWinner? mainBetWinner, MainBetOptionsType? userMainBetOption)
        {
            if((mainBetWinner == MainBetWinner.Home && userMainBetOption == MainBetOptionsType.Home) 
                || (mainBetWinner == MainBetWinner.Away && userMainBetOption == MainBetOptionsType.Away) 
                || (mainBetWinner == MainBetWinner.Draw && userMainBetOption == MainBetOptionsType.Draw)) {
                    return true;
                } else {
                    return false;
                }  
        }

       public Double GetMatchOddByBet(Match match, Bet bet)
        {
            var betDetails = GetBetDetailsByBet(bet);
            var betDetailsUniq = betDetails.FirstOrDefault(betDetail => betDetail.MatchId == match.Id);
            if(betDetailsUniq.IsMainBet) {
                var mainBetOption = matchHelperService.GetMainBetOptionByMatchId(match.Id);
                var mainBetOptionsType = betDetailsUniq.MainBetOptionsType;
                if(mainBetOptionsType == MainBetOptionsType.Home) {
                    return mainBetOption.HomeOdd;
                } else if(mainBetOptionsType == MainBetOptionsType.Away) {
                    return mainBetOption.AwayOdd;
                } else {
                    return mainBetOption.DrawOdd;
                }
            } else {
                var subBetSelectId = betDetailsUniq.SubBetSelectId;
                var subBetSelect = _context.SubBetSelects.FirstOrDefault(subBetSelect => subBetSelect.Id == subBetSelectId);
                return subBetSelect.Odd;
            }
            return 0;
        }
        
        public string GetBetTypeStringToBetAndMatch(Match match, Bet bet)
        {
            var betDetails = GetBetDetailsByBet(bet);
            var betDetailsUniq = betDetails.FirstOrDefault(betDetail => betDetail.MatchId == match.Id);
            if(betDetailsUniq.IsMainBet) {
                return "Maç Sonucu";
            } else {
                var SubBetOptionId = betDetailsUniq.SubBetOptionId;
                var subBetOption = _context.SubBetOptions.FirstOrDefault(subBetOption => subBetOption.Id == SubBetOptionId);
                return subBetOption.Name;
            }
        }

        public string GetUserBetTypeStringToBetAndMatch(Match match, Bet bet)
        {
            var betDetails = GetBetDetailsByBet(bet);
            var betDetailsUniq = betDetails.FirstOrDefault(betDetail => betDetail.MatchId == match.Id);
            if(betDetailsUniq.IsMainBet) {
                var mainBetOptionsType = betDetailsUniq.MainBetOptionsType;
                if(mainBetOptionsType == MainBetOptionsType.Home) {
                    return "1";
                } else if(mainBetOptionsType == MainBetOptionsType.Away) {
                    return "2";
                } else {
                    return "X";
                }
            } else {
                var subBetSelectId = betDetailsUniq.SubBetSelectId;
                var subBetSelect = _context.SubBetSelects.FirstOrDefault(subBetSelect => subBetSelect.Id == subBetSelectId);
                return subBetSelect.Name;
            }
        }

    }
}