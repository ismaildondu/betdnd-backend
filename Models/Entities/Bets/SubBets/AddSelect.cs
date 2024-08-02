using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BetDND.Enums;

namespace BetDND.Models
{
    public class AddSelect 
    {
        public string Name { get; set; }
        public double Odd { get; set; }
        public int SubBetOptionId { get; set; }
    }
}