using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BetDND.Models;
using BetDND.Enums;
using BetDND.Data;
using BetDND.Services;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using System.Text.Json;


namespace BetDND.Controllers
{
    [Route("api/administration")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdministrationController : ControllerBase {
        private readonly DataContext _context;
        private readonly UserHelperService userHelperService;
        private readonly BetService betService;

        public AdministrationController(DataContext context)
        {
            _context = context;
            userHelperService = new UserHelperService(_context);
            betService = new BetService(_context);
        }
        [HttpGet("users")]
        public IActionResult GetUsers ()
        {
            return Ok(GetAllUsers());
        }


        [HttpGet("matches")]
        public IActionResult GetMatches ()
        {
            var matches = _context.Matches.ToList();
            matches = matches.OrderByDescending(m => m.Id).ToList();
            return Ok(matches);
        }

        [HttpGet("finishmatch/{id}")]
        public IActionResult FinishMatch (int id)
        {
            var match = _context.Matches.FirstOrDefault(m => m.Id == id);
            var mainBetOption = _context.MainBetOptions.FirstOrDefault(m => m.MatchId == id);
            var subBetObjects = new List<object>();
            var subBetCategories = _context.SubBetCategories.Where(s => s.MatchId == id).ToList();
            foreach (var subBetCategory in subBetCategories)
            {
                var subBetOptions = _context.SubBetOptions.Where(s => s.SubBetCategoryId == subBetCategory.Id).ToList();
                var subBetOptionsObjects = new List<object>();
                foreach (var subBetOption in subBetOptions)
                {
                    var subBetSelects = _context.SubBetSelects.Where(s => s.SubBetOptionId == subBetOption.Id).ToList();
                    var subBetSelectsObjects = new List<object>();
                    foreach (var subBetSelect in subBetSelects)
                    {
                        var subBetSelectObject = new {
                            Id = subBetSelect.Id,
                            Name = subBetSelect.Name,
                            Odd = subBetSelect.Odd
                        };
                        subBetSelectsObjects.Add(subBetSelectObject);
                    }
                    var subBetOptionObject = new {
                        Id = subBetOption.Id,
                        Name = subBetOption.Name,
                        Description = subBetOption.Description,
                        SubBetSelects = subBetSelectsObjects
                    };
                    subBetOptionsObjects.Add(subBetOptionObject);
                } 
                var subBetCategoryObject = new {
                    Id = subBetCategory.Id,
                    Name = subBetCategory.Name,
                    SubBetOptions = subBetOptionsObjects
                };  
                subBetObjects.Add(subBetCategoryObject);
            }
            var matchObject = new {
                Id = match.Id,
                Sport = match.Sport,
                Mbs = match.Mbs,
                HomeTeam = match.HomeTeam,
                AwayTeam = match.AwayTeam,
                Country = match.Country,
                CountryCode = match.CountryCode,
                Date = match.Date,
                Time = match.Time,
                League = match.League,
                IsFinished = match.IsFinished,
                HomeOdd = mainBetOption.HomeOdd,
                AwayOdd = mainBetOption.AwayOdd,
                DrawOdd = mainBetOption.DrawOdd,
                SubBetCategories = subBetObjects
            };
            return Ok(matchObject);
         
           
        }

        [HttpGet("match/{id}")]
        public IActionResult GetMatch (int id)
        {
            var match = _context.Matches.FirstOrDefault(m => m.Id == id);
            return Ok(match);
        }

        [HttpPost("userchangeban")]
        public IActionResult GetUsers ([FromBody] EmailInput email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email.Email);
            user.IsBanned = !user.IsBanned;
            _context.SaveChanges();
            return Ok(GetAllUsers());
        }

        [HttpPost("useraddbalance")]
        public IActionResult GetUsers ([FromBody] BalanceInput balanceInput)
        {   
            var user = _context.Users.FirstOrDefault(u => u.Email == balanceInput.Email);
            user.Balance += balanceInput.Amount;
            _context.SaveChanges();
            return Ok(GetAllUsers());
        }
        
        private List<User> GetAllUsers() {
            var users = _context.Users.ToList();
            users = users.OrderByDescending(u => u.Id).ToList();
            return users;
        }

        [HttpPost("addcategory")]
        public IActionResult AddCategory([FromBody] AddCategory addCategory)
        {
            try{
                SubBetCategory category = new SubBetCategory {
                    MatchId = addCategory.MatchId,
                    Name = addCategory.Name
                };
                _context.SubBetCategories.Add(category);
                _context.SaveChanges();
                return Ok(category.Id);
            } catch (System.Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("addoption")]
        public IActionResult AddOption([FromBody] AddOption addOption)
        {
            try{
                SubBetOption option = new SubBetOption {
                    SubBetCategoryId = addOption.CategoryId,
                    Name = addOption.Name,
                    Description = addOption.Description
                };
                _context.SubBetOptions.Add(option);
                _context.SaveChanges();
                return Ok(option.Id);
            } catch (System.Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("addselect")]
        public IActionResult AddSelect([FromBody] AddSelect addSelect)
        {
            try{
                SubBetSelect select = new SubBetSelect {
                    SubBetOptionId = addSelect.SubBetOptionId,
                    Name = addSelect.Name,
                    Odd = addSelect.Odd
                };
                _context.SubBetSelects.Add(select);
                _context.SaveChanges();
                return Ok();
            } catch (System.Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("addmatch")]
        public IActionResult AddMatch([FromBody] AddMatch addMatch)
        {
            try{
                Match match = new Match {
                    Sport = addMatch.MatchSports,
                    Mbs = addMatch.MBSRule,
                    HomeTeam = addMatch.HomeTeam,
                    AwayTeam = addMatch.AwayTeam,
                    Country = addMatch.Country,
                    CountryCode = addMatch.CountryCode,
                    Date = addMatch.Date,
                    Time = addMatch.Time,
                    League = addMatch.League,
                    IsFinished = false
                };
                _context.Matches.Add(match);
                _context.SaveChanges();

                MainBetOptions mainBetOption = new MainBetOptions {
                    MatchId = match.Id,
                    HomeOdd = addMatch.MainBetOptions.HomeOdd,
                    AwayOdd = addMatch.MainBetOptions.AwayOdd,
                    DrawOdd = addMatch.MainBetOptions.DrawOdd
                };

                _context.MainBetOptions.Add(mainBetOption);
                _context.SaveChanges();

                return Ok();
            } catch (System.Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("editmatch")]
        public IActionResult AddMatch([FromBody] EditMatch EditMatch)
        {
            try{
                Match match = _context.Matches.FirstOrDefault(m => m.Id == EditMatch.Id);
                match.Sport = EditMatch.MatchSports;
                match.Mbs = EditMatch.MBSRule;
                match.HomeTeam = EditMatch.HomeTeam;
                match.AwayTeam = EditMatch.AwayTeam;
                match.Country = EditMatch.Country;
                match.CountryCode = EditMatch.CountryCode;
                match.Date = EditMatch.Date;
                match.Time = EditMatch.Time;
                match.League = EditMatch.League;
                match.IsFinished = EditMatch.IsFinished;
                _context.SaveChanges();

                return Ok();
            } catch (System.Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("finishmatch")]
        public IActionResult FinishMatch([FromBody] FinishMatch FinishMatch)
        {
            try{
                Match match = _context.Matches.FirstOrDefault(m => m.Id == FinishMatch.MatchId);
                match.IsFinished = true;
                _context.SaveChanges();

                MainBetOptions mainBetOption = _context.MainBetOptions.FirstOrDefault(m => m.MatchId == FinishMatch.MatchId);
                mainBetOption.MainBetWinner = FinishMatch.MainBet;
                _context.SaveChanges();
                
                foreach (var winSubBet in FinishMatch.WinSubBets) {
                    var subBetOptions = _context.SubBetOptions.FirstOrDefault(s => s.Id == winSubBet.SubBetOptionId);
                    subBetOptions.SubBetSelectWinnerId = winSubBet.SubBetSelectId;
                    _context.SaveChanges();
                }
                var betsDetails = _context.BetDetails.Where(b => b.MatchId == FinishMatch.MatchId).ToList();
                List<Bet> bets = new List<Bet>();
                foreach (var betDetail in betsDetails) {
                    var bet = _context.Bets.FirstOrDefault(b => b.Id == betDetail.BetId);
                    bets.Add(bet);
                }
                var uniqBets = bets.Distinct().ToList();
                foreach (var bet in uniqBets) {
                    _context.Database.BeginTransaction();
                    int userId = bet.UserId;
                    var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                    decimal amount = (decimal)betService.GetBetTotalOdd(bet) * bet.Amount;
                    BetStatus betStatus = betService.GetBetStatus(bet);
                    if (betStatus == BetStatus.Won) {
                        user.Balance += amount;
                    }
                    Bet getBet = _context.Bets.FirstOrDefault(b => b.Id == bet.Id);
                    getBet.Status = betStatus;
                    _context.SaveChanges();
                    _context.Database.CommitTransaction();
                }
                return Ok();
            } catch (System.Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("addbulkcategories")]
        public IActionResult AddBulkCategories([FromBody] AddBulk[] AddBulk)
        {
            try{
               foreach (var addBulk in AddBulk) {
                    SubBetCategory category = new SubBetCategory {
                        MatchId = addBulk.AddCategory.MatchId,
                        Name = addBulk.AddCategory.Name
                    };
                    _context.SubBetCategories.Add(category);
                    _context.SaveChanges();

                    foreach(var optionsBulk in addBulk.AddOptionsBulk) {
                        SubBetOption option = new SubBetOption {
                            SubBetCategoryId = category.Id,
                            Name = optionsBulk.Name,
                            Description = optionsBulk.Description
                        };
                        _context.SubBetOptions.Add(option);
                        _context.SaveChanges();

                        foreach(var selectBulk in optionsBulk.AddSelects) {
                            SubBetSelect select = new SubBetSelect {
                                SubBetOptionId = option.Id,
                                Name = selectBulk.Name,
                                Odd = selectBulk.Odd
                            };
                            _context.SubBetSelects.Add(select);
                            _context.SaveChanges();
                        }
                    }
                }
                return Ok();
            } catch (System.Exception e) {
                return BadRequest(e.Message);
            }
        }
        

        

       
    }
}


