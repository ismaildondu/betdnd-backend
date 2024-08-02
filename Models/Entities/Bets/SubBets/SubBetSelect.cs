using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetDND.Models 
{
    public class SubBetSelect {
        /// <summary>
        /// Gets or sets the ID of the sub bet selection.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the sub bet selection.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the odd of the sub bet selection.
        /// </summary>
        public double Odd { get; set; }

        /// <summary>
        /// Gets or sets the sub bet option associated with the sub bet selection.
        /// </summary>
        public SubBetOption SubBetOption { get; set; }
        
        public int SubBetOptionId { get; set; }

        public ICollection<BetDetail> BetDetails { get; set; }

    }
}