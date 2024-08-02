using BetDND.Models;
using BetDND.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Security.Claims;

namespace BetDND.Services
{
    public class LoginService
    {
        private readonly DataContext _context;
        private readonly PasswordService passwordService;
        private readonly UserHelperService userHelperService;
        private readonly TokenService tokenService;

        public LoginService(DataContext context)
        {
            _context = context;
            passwordService = new PasswordService();
            userHelperService = new UserHelperService(_context);
            tokenService = new TokenService();
        }

        public string AuthenticateUser (AuthenticationInput authenticationInput)
        {
            User? user = userHelperService.GetUserByAuthenticationInput(authenticationInput);
            if (user == null) {
                throw new Exception(MessageService.InvalidCredentials);
            }
            string token = tokenService.GenerateToken(user);
            SetUserToken(user, token);
            return token;
        }

        private void SetUserToken (User user, string token)
        {
            user.AuthToken = token;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void IsCurrentUserTokenValid (string token)
        {
            User? user = userHelperService.GetUserByToken(token);
            if (user == null || user.AuthToken != token) {
                throw new Exception(MessageService.InvalidToken);
            }
        }
    }
}
