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
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase {
        private readonly DataContext _context;
        private readonly LoginService loginService;
        private readonly UserHelperService userHelperService;
        private readonly BetService betService;
        private readonly MatchHelperService matchHelperService;
        private readonly NewBetService newBetService;
        private readonly TokenService tokenService;

        public UserController(DataContext context)
        {
            _context = context;
            loginService = new LoginService(_context);
            userHelperService = new UserHelperService(_context);
            betService = new BetService(_context);
            matchHelperService = new MatchHelperService(_context);
            newBetService = new NewBetService(_context);
            tokenService = new TokenService();
        }

        [HttpGet("/api/user")]
        public IActionResult GetMe ()
        {
            try {
                string token = tokenService.GetToken(Request.Headers["Authorization"]);
                User user = userHelperService.GetUserByToken(token);
                var userBets = betService.GetBetsByUser(user);
                List<object> userBetsObject = new List<object>();

                foreach (var bet in userBets)
                {
                    var betDetailObject = new {
                            date = bet.CreatedAt,
                            totalOdd = betService.GetBetTotalOdd(bet),
                            status = betService.BetStatusToString(bet.Status),
                            amount = bet.Amount,
                            id = bet.Id,
                        };
                    userBetsObject.Add(betDetailObject);
                }
                userBetsObject.Reverse();
                return Ok(new {
                    id = user.Id,
                    name = user.NameSurname,
                    balance = user.Balance,
                    bets = userBetsObject
                });
            } catch (System.Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("newbet")]
        public IActionResult AddBet([FromBody] AddBet addBet) {
            try {
                string token = tokenService.GetToken(Request.Headers["Authorization"]);
                loginService.IsCurrentUserTokenValid(token);
                User user = userHelperService.GetUserByToken(token);
                int betId = newBetService.AddNewBets(addBet,user);
                return Ok(new {
                    id = betId
                });
                
            } catch (System.Exception e) {
                return BadRequest(e.Message);
            }

        }

        [HttpGet("bet/{id}")]
        public IActionResult GetBetDetailById(int id)
        {
            try {
                string token = tokenService.GetToken(Request.Headers["Authorization"]);
                loginService.IsCurrentUserTokenValid(token);
                User user = userHelperService.GetUserByToken(token);
                Bet bet = betService.GetBetByIdAndUser(id, user);
                var amount = bet.Amount;
                var status = betService.BetStatusToString(bet.Status);
                var totalOdd = betService.GetBetTotalOdd(bet);
                var maxWin = amount * (decimal)totalOdd;
                var matchList = betService.GetMatchesByBet(bet);
                List<object> matchListObject = new List<object>();
                foreach (var match in matchList)
                {
                    int mbs = 1;
                    switch(match.Mbs) {
                        case MBSRule.One:
                            mbs = 1;
                            break;
                        case MBSRule.Two:
                            mbs = 2;
                            break;
                        case MBSRule.Three:
                            mbs = 3;
                            break;
                        default:
                            mbs = 1;
                            break;
                    }

                    var matchObject = new {
                        id = match.Id,
                        sport = matchHelperService.GetSportName(match.Sport),
                        odd = betService.GetMatchOddByBet(match, bet),
                        date = match.Date,
                        time = match.Time,
                        homeTeam = match.HomeTeam,
                        awayTeam = match.AwayTeam,
                        betType = betService.GetBetTypeStringToBetAndMatch(match, bet),
                        userBetType = betService.GetUserBetTypeStringToBetAndMatch(match, bet),
                        status = betService.BetStatusToString(betService.GetMatchStatusByBet(match, bet)),
                        mbs = mbs
                    };
                    matchListObject.Add(matchObject);
                }
                var betDetailObject = new {
                    amount = amount,
                    status = status,
                    totalOdd = totalOdd,
                    maxWin = maxWin,
                    matches = matchListObject,
                    id = bet.Id
                };
                return Ok(betDetailObject);
            } catch (System.Exception e) {
                return BadRequest();
            }
        }
       
    }   
}