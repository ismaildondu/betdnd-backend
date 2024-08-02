using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace BetDND.Models
{
    public class AuthenticationInput
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [MinLength(8)]
        [Required]
        public string Password { get; set; }
    }
}