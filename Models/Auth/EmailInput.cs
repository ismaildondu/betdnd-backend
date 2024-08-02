using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace BetDND.Models
{
    public class EmailInput
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}