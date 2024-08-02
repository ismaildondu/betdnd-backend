using BetDND.Models;
using BetDND.Data;
using System.Linq;
using BetDND.Enums;

namespace BetDND.Services
{
    public class NewBetService {
        public readonly DataContext _context;
        public readonly MatchHelperService matchHelperService;
        public readonly SubBetService subBetservice;
        public readonly BetService betService;

        public NewBetService(DataContext context)
        {
            _context = context;
            matchHelperService = new MatchHelperService(_context);
            subBetservice = new SubBetService(_context);
            betService = new BetService(_context);
        }

        public int AddNewBets(AddBet addBet, User user) {
            IsBetValid(addBet, user);   
            return betService.AddBet(addBet, user);
        }

        private void IsBetValid(AddBet addBet,User user)
        {
            foreach(NewBet newBet in addBet.newBets) {
                if(!IsAmountValid(addBet, user)) {
                    throw new System.Exception(MessageService.InvalidAmount);
                }
                if(!IsMatchIdValid(newBet.matchId)) {
                    throw new System.Exception(MessageService.InvalidMatchId);
                }
                if(!IsMatchIdUniqInNewBets(addBet.newBets, newBet.matchId)) {
                    throw new System.Exception(MessageService.MatchIdNotUniq);
                }
                if(newBet.isMain && !IsMainBetValid(newBet.optionName)) {
                    throw new System.Exception(MessageService.InvalidMainBet);
                }
                if(!newBet.isMain && !IsSelectValid(newBet)) {
                    throw new System.Exception(MessageService.InvalidSelect);
                }
            }
        }

        private bool IsAmountValid(AddBet addBet, User user) {
            if(addBet.amount < 1) {
                return false;
            }
            if(addBet.amount > user.Balance) {
                return false;
            }
            return true;
        }

        private bool IsSelectValid(NewBet newBet) {
            Match match = matchHelperService.GetMatchById(newBet.matchId);
            int selectID = newBet.select ?? 0;
            SubBetSelect? subBetSelect = _context.SubBetSelects.FirstOrDefault(s => s.Id == selectID);
            if(subBetSelect == null) {
                return false;
            }
            SubBetOption subBetOption = subBetservice.GetSubBetOptionById(subBetSelect.SubBetOptionId);
            SubBetCategory subBetCategory = _context.SubBetCategories.FirstOrDefault(s => s.Id == subBetOption.SubBetCategoryId);
            if(subBetCategory.MatchId != match.Id) {
                return false;
            }
            return true;
        }

        private bool IsMatchIdValid(int matchId)
        {
            Match match = matchHelperService.GetMatchById(matchId);
            if(match != null || match.IsFinished == false) {
                return true;
            }
            return false;
        }

        private bool IsMainBetValid(string optionName) {
            if(optionName == "1" || optionName == "X" || optionName == "2") {
                return true;
            }
            return false;
        }

        private bool IsMatchIdUniqInNewBets(NewBet[] newBets, int matchId)
        {
            int count = 0;
            foreach(NewBet newBet in newBets) {
                if(newBet.matchId == matchId) {
                    count++;
                }
            }
            if(count == 1) {
                return true;
            } else {
                return false;
            }
        }
    }
}