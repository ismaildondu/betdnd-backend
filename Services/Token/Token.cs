using BetDND.Models;
using BetDND.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BetDND.Services
{
    public class TokenService
    {
        public string GenerateToken(User user)
        {
            var token = GenerateTokenObject(user);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        private JwtSecurityToken GenerateTokenObject(User user)
        {
            var token = new JwtSecurityToken(
                issuer: TokenConfigService.Issuer,
                audience: TokenConfigService.Audience,
                expires: DateTime.Now.AddDays(TokenConfigService.ExpirationDays),
                signingCredentials: GetSigningKey(),
                claims: new Claim[] {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
                 }
            );
            return token;
        }

        private SigningCredentials GetSigningKey()
        {
            var securityKey = GetSecurityKey();
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            return signingCredentials;
        }

        private SecurityKey GetSecurityKey()
        {
            var key = Encoding.ASCII.GetBytes(TokenConfigService.Key);
            var securityKey = new SymmetricSecurityKey(key);
            return securityKey;
        }

        public bool ValidateToken(string token)
        {
            try {
                GetValidatedToken(token);
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public SecurityToken GetValidatedToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSecurityKey(),
                ValidateLifetime = true,
                ValidIssuer = TokenConfigService.Issuer,
                ValidAudience = TokenConfigService.Audience,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            if (validatedToken == null) {
                throw new Exception(MessageService.InvalidToken);
            }
            return validatedToken;
        }

        public string? GetEmailFromToken(string token)
        {
            try {
                SecurityToken validatedToken = GetValidatedToken(token);
                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            } catch {
                return null;
            }
        }

        public string GetToken(string tokenString)
        {
            var token = tokenString.Split(" ")[1];
            return token;
        }

    }
}
