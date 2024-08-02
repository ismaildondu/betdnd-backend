using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetDND.Models 
{
    public class SubBetOption {
        /// <summary>
        /// Gets or sets the ID of the sub-bet option.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the sub-bet option.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the sub-bet option.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of sub-bet selects associated with the sub-bet option.
        /// </summary>
        public ICollection<SubBetSelect> SubBetSelects { get; set; }

        /// <summary>
        /// Gets or sets the sub-bet category associated with the sub-bet option.
        /// </summary>
        public SubBetCategory SubBetCategory { get; set; }

        public int SubBetCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the sub-bet select winner associated with the sub-bet option.
        /// </summary>
        public SubBetSelect? SubBetSelectWinner { get; set; }

        public int? SubBetSelectWinnerId { get; set; }

        public ICollection<BetDetail> BetDetails { get; set; }

    }
}