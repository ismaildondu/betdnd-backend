using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace BetDND.Models
{
    public class RegistrationInput : AuthenticationInput
    {
        [Required]
        public string NameSurname { get; set; }
    }
}