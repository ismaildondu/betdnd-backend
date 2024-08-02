using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BetDND.Enums;

namespace BetDND.Models 
{
    public class BetDetail {
        /// <summary>
        /// Gets or sets the ID of the bet detail.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the bet detail is the main bet.
        /// </summary>
        public bool IsMainBet { get; set; }

        public MainBetOptionsType? MainBetOptionsType { get; set; }

        /// <summary>
        /// Gets or sets the odd of the bet detail.
        /// </summary>
        public double Odd { get; set; }

        /// <summary>
        /// Gets or sets the bet associated with the bet detail.
        /// </summary>
        public Bet Bet { get; set; }

        public int BetId { get; set; }

        /// <summary>
        /// Gets or sets the match associated with the bet detail.
        /// </summary>
        public Match Match { get; set; }

        public int MatchId { get; set; }

        /// <summary>
        /// Gets or sets the sub bet category of the bet detail.
        /// </summary>
        public SubBetCategory? SubBetCategory { get; set; }

        public int? SubBetCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the sub bet option of the bet detail.
        /// </summary>
        public SubBetOption? SubBetOption { get; set; }
        
        public int? SubBetOptionId { get; set; }

        /// <summary>
        /// Gets or sets the sub bet select of the bet detail.
        /// </summary>
        public SubBetSelect? SubBetSelect { get; set; }

        public int? SubBetSelectId { get; set; }
    }
}