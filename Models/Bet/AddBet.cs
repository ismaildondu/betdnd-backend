using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetDND.Models 
{
    public class AddBet {
        public NewBet[] newBets { get; set; }
        public decimal amount { get; set; }
    }
}