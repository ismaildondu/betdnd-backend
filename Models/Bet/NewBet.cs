using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetDND.Models 
{
    public class NewBet {
        public bool isMain { get; set; }
        public int matchId { get; set; }
        public string optionName { get; set; }
        public int? select { get; set; }
    }
}