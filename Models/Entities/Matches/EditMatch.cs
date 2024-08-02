using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BetDND.Enums;

namespace BetDND.Models
{
    public class EditMatch {
        public int Id { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        [RegularExpression(@"\d{4}-\d{2}-\d{2}")]
        public string Date { get; set; }
        [RegularExpression(@"\d{2}:\d{2}")]
        public string Time { get; set; }
        public string League { get; set; }
        public MatchSports MatchSports { get; set; }
        public MBSRule MBSRule { get; set; }
        public bool IsFinished { get; set; }
    }
}