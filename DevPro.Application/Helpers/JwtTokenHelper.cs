using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevPro.Application.Helpers
{

    public class JwtTokenHelper
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _securityKey; 

        public JwtTokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"] ?? ""));
        }

        public string CreateTokenWithRole(string email, string userId, IList<string> roles)
        {
            var claims = new List<Claim>
            {
               new Claim(JwtRegisteredClaimNames.NameId, userId),
               //new Claim(JwtRegisteredClaimNames.Email, email),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var creds = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(6),
                SigningCredentials = creds,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


    }
}
