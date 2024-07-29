
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.BusinessLayer.Interfaces;
using API.Models;
using Microsoft.IdentityModel.Tokens;

namespace API.BusinessLayer.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(Users user)
        {
            try
            {
                var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access token key from appsettings");
                if(tokenKey.Length < 64) throw new Exception("Token needs to be longer");   
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.XPCode),
                    new(ClaimTypes.NameIdentifier, user.UserName)
                };

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}