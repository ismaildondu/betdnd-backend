using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetDND.Models 
{
    public class SubBetCategory {
        /// <summary>
        /// Gets or sets the ID of the sub bet category.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the sub bet category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the match associated with the sub bet category.
        /// </summary>
        public Match Match { get; set; }

        public int MatchId { get; set; }

        /// <summary>
        /// Gets or sets the collection of sub bet options associated with the sub bet category.
        /// </summary>
        public ICollection<SubBetOption> SubBetOptions { get; set; }

        public ICollection<BetDetail> BetDetails { get; set; }
    }
}