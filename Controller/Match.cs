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
    [Route("api/match")]
    [ApiController]
    public class MatchController : ControllerBase {
        private readonly DataContext _context;
        private readonly MatchHelperService matchHelperService;

        public MatchController(DataContext context)
        {
            _context = context;
            matchHelperService = new MatchHelperService(_context);
        }


        [HttpGet("all")]
        public IActionResult GetMatches ()
        {
          var matches = matchHelperService.GetUnfinishedMatches();
          var matchesObject = this.GetMatchesWithObjects(matches);
          return Ok(matchesObject);
        }

        private object GetMatchesWithObjects(List<Match> matches)
        {
            var matchList = new List<object>();
            foreach (var match in matches)
            {
                var matchObject = matchHelperService.CreateMatchObject(match);
                matchList.Add(matchObject);
            }
            var matchListToObject = new { items=matchList };
            return matchListToObject;
        }
        
        
    }
}
