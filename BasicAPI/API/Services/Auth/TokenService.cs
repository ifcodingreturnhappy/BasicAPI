using DataLayer.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly TimeSpan TOKEN_LIFESPAN = TimeSpan.FromMinutes(60 * 2);
        private readonly AuthSettings _settings;

        public TokenService(AuthSettings settings)
        {
            _settings = settings;
        }

        public string GenerateToken(User user, out DateTime? expiryDate)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.AUTH_SECRET);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.Add(TOKEN_LIFESPAN),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            expiryDate = tokenDescriptor.Expires;
            return tokenHandler.WriteToken(token);
        }

    }
}
