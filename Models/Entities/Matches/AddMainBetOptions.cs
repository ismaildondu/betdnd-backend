using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BetDND.Enums;

namespace BetDND.Models
{
    public class AddMainBetOptions {
        public double HomeOdd { get; set; }
        public double AwayOdd { get; set; }
        public double DrawOdd { get; set; }
    }
}