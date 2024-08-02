using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetDND.Models 
{
    public class Bet {
        /// <summary>
        /// Gets or sets the ID of the bet.
        /// </summary>
        [Key]
        public int Id { get; set; }

        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the status of the bet.
        /// </summary>
        public BetDND.Enums.BetStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the amount of the bet.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the collection of bet details associated with the bet.
        /// </summary>
        public ICollection<BetDetail> BetDetails { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the bet.
        /// </summary>
        public User User { get; set; }

        public int UserId { get; set; }

        public Bet()
        {
            this.CreatedAt = DateTime.Now.ToString("dd.MM.yyyy - HH:mm");
        }
        
    }
}