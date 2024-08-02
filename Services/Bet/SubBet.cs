using BetDND.Models;
using BetDND.Data;
using System.Linq;
using BetDND.Enums;

namespace BetDND.Services
{
    public class SubBetService {
        private readonly DataContext _context;

        public SubBetService(DataContext context)
        {
            _context = context;
        }

        public List<object> GetSubBetOptions(int matchId)
        {
            var subBetOptions = new List<object>();
            var subBetCategories = _context.SubBetCategories.Where(s => s.MatchId == matchId).ToList();
            foreach (var subBetCategory in subBetCategories)
            {
                var options = new List<object>();
                var subBetOptionsList = _context.SubBetOptions.Where(s => s.SubBetCategoryId == subBetCategory.Id).ToList();
                foreach (var subBetOption in subBetOptionsList)
                {
                        var optionList = GetSubBetSelects(subBetOption.Id);
                        var subBetOptionObject = new { name = subBetOption.Name, description = subBetOption.Description, id = subBetOption.Id , options = optionList};
                        options.Add(subBetOptionObject);
                }
                var betOptionObject = new { title = subBetCategory.Name, id = subBetCategory.Id, options = options };
                subBetOptions.Add(betOptionObject);
                }
            return subBetOptions;
        }

        public List<object> GetSubBetSelects(int subBetOptionId)
        {
            var optionList = new List<object>();
            var subBetSelects = _context.SubBetSelects.Where(s => s.SubBetOptionId == subBetOptionId).ToList();
            foreach (var subBetSelect in subBetSelects)
            {
                var option = new { name = subBetSelect.Name, id = subBetSelect.Id, odds = subBetSelect.Odd  };
                optionList.Add(option);
            }
            return optionList;
        }

        public SubBetOption GetSubBetOptionById(int subBetOptionId)
        {
            return _context.SubBetOptions.FirstOrDefault(s => s.Id == subBetOptionId);
        }




    }
}