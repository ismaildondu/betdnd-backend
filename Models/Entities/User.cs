using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetDND.Models
{
    public class User
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        [Key]
        public int Id { get; set; }

        public string NameSurname { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string Email { get; set; }

        public string? AuthToken { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user's balance.
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is an admin.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is banned.
        /// </summary>
        public bool IsBanned { get; set; }

        /// <summary>
        /// Gets or sets the collection of bets associated with the user.
        /// </summary>
        public ICollection<Bet> Bets { get; set; }
    }
}