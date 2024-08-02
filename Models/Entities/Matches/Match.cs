using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BetDND.Enums;

namespace BetDND.Models
{
    public class Match {
        /// <summary>
        /// Gets or sets the ID of the match.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the sport of the match.
        /// </summary>
        public BetDND.Enums.MatchSports Sport { get; set; }

        public bool IsFinished { get; set; }

        /// <summary>
        /// Gets or sets the MBS rule of the match.
        /// </summary>
        public BetDND.Enums.MBSRule Mbs { get; set; }

        /// <summary>
        /// Gets or sets the home team of the match.
        /// </summary>
        public string HomeTeam { get; set; }

        /// <summary>
        /// Gets or sets the away team of the match.
        /// </summary>
        public string AwayTeam { get; set; }

        /// <summary>
        /// Gets or sets the country of the match.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the country code of the match.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the date of the match.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the time of the match.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets the league of the match.
        /// </summary>
        public string League { get; set; }

        /// <summary>
        /// Gets or sets the main bet options of the match.
        /// </summary>
        public MainBetOptions MainBetOptions { get; set; }

        /// <summary>
        /// Gets or sets the bet details of the match.
        /// </summary>
        public ICollection<BetDetail> BetDetails { get; set; }

        /// <summary>
        /// Gets or sets the sub bet categories of the match.
        /// </summary>
        public ICollection<SubBetCategory> SubBetCategories { get; set; }
    }
}