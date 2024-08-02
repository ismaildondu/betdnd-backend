using BetDND.Models;
using BetDND.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace BetDND.Services
{
    public class UserHelperService
    {
        private readonly DataContext _context;
        private readonly PasswordService passwordService;
        private readonly TokenService tokenService;
        
        public UserHelperService(DataContext context)
        {
            _context = context;
            passwordService = new PasswordService();
            tokenService = new TokenService();
        }

        public User? GetUserByAuthenticationInput(AuthenticationInput authenticationInput)
        {
            User? user = GetUserByEmail(authenticationInput.Email);
            if (IsFailedAuthentication(authenticationInput, user)) {
                return null;
            }
            if(user.IsBanned){
                return null;
            }
            return user;
        }

        private bool IsFailedAuthentication(AuthenticationInput authenticationInput, User user)
        {
            return user == null || !passwordService.VerifyPassword(authenticationInput.Password, user.Password);
        }

        public User? GetUserByEmail(string email)
        {
            User? user = _context.Users.Where(user => user.Email == email).FirstOrDefault();
            return user;
        }

        public User? GetUserByToken(string token)
        {
            string? email = tokenService.GetEmailFromToken(token);
            User? user = GetUserByEmail(email);
            return user;
        }

        public bool IsEmailTaken(string email)
        {
            return GetUserByEmail(email) != null;
        }

        public bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public bool IsValidPassword(string password)
        {   
            password = password.Trim();
            return password.Length >= 8;
        }
    }
}
