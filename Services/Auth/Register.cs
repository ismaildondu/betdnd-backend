using BetDND.Models;
using BetDND.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace BetDND.Services
{
    public class RegisterService
    {
        private readonly DataContext _context;
        private readonly PasswordService passwordService;
        private readonly UserHelperService userHelperService;

        public RegisterService(DataContext context)
        {
            _context = context;
            passwordService = new PasswordService();
            userHelperService = new UserHelperService(_context);
        }

        public User CreateUser(AuthenticationInput authenticationInput, string nameSurname)
        {
            try {
                ValidateUser(authenticationInput, nameSurname);
                User user = GenerateUser(authenticationInput, nameSurname);
                SaveUser(user);
                return user;
            } catch (System.Exception e) {
                throw e;
            }
        }

        private void ValidateUser(AuthenticationInput authenticationInput, string nameSurname)
        {
            ValidateEmailFormat(authenticationInput.Email);
            ValidatePassword(authenticationInput.Password);
            EmailExists(authenticationInput.Email);
            ValidateNameSurname(nameSurname);
        }

        private void ValidateNameSurname(string nameSurname)
        {
            if (nameSurname == null || nameSurname.Length < 5) {
                throw new Exception(MessageService.NameSurnameTooShort);
            }
        }

        private void ValidateEmailFormat(string email)
        {
            if (!userHelperService.IsValidEmail(email)) {
                throw new Exception(MessageService.InvalidEmailFormat);
            }
        }

        private void ValidatePassword(string password)
        {
            if (!userHelperService.IsValidPassword(password)) {
                throw new Exception(MessageService.PasswordTooShort);
            }
        }

        private void EmailExists(string email)
        {
            if(userHelperService.IsEmailTaken(email)) {
                throw new Exception(MessageService.EmailExists);
            }
        }

        private User GenerateUser(AuthenticationInput authenticationInput, string nameSurname)
        {
            return new User {
                Email = authenticationInput.Email,
                NameSurname = nameSurname,
                Password = passwordService.HashPassword(authenticationInput.Password),
                Balance = 0,
                IsAdmin = true,
                IsBanned = false
            };
        }

        private void SaveUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
