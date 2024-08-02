using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace BetDND.Models
{
    public class BalanceInput : EmailInput
    {
        [Required]
        public decimal Amount { get; set; }
    }
}