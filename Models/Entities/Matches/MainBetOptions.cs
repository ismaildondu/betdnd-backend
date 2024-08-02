using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BetDND.Enums;

namespace BetDND.Models
{
    public class MainBetOptions
    {
        /// <summary>
        /// Gets or sets the ID of the main bet option.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the home odd for the main bet option.
        /// </summary>
        public double HomeOdd { get; set; }

        /// <summary>
        /// Gets or sets the away odd for the main bet option.
        /// </summary>
        public double AwayOdd { get; set; }

        /// <summary>
        /// Gets or sets the draw odd for the main bet option.
        /// </summary>
        public double DrawOdd { get; set; }

        /// <summary>
        /// Gets or sets the winner for the main bet option.
        /// </summary>
        public MainBetWinner? MainBetWinner { get; set; }

        /// <summary>
        /// Gets or sets the match associated with the main bet option.
        /// </summary>
        public Match Match { get; set; }
        public int MatchId { get; set; }
    }
}