using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BetDND.Enums;

namespace BetDND.Models
{
    public class AddCategory 
    {
        public int MatchId { get; set; }
        public string Name { get; set; }
    }
}