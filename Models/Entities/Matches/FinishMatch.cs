using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BetDND.Enums;

namespace BetDND.Models
{
    public class FinishMatch 
    {
        public int MatchId { get; set; }
        public MainBetWinner MainBet { get; set; }
        public List<WinSubBet> WinSubBets { get; set; }
    }

    public class WinSubBet 
    {
        public int SubBetOptionId { get; set; }
        public int SubBetSelectId { get; set; }
    }
}