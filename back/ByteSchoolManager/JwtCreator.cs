using ClubsBack;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClubsBack
{
    public class JwtCreator
    {
        private readonly AuthOptions _authOptions;
        public JwtCreator(AuthOptions authOptions)
        {
            _authOptions = authOptions;
        }

        public string Create(string role, int id)
        {
            var claims = new List<Claim> { new(ClaimTypes.Role, role), new(ClaimTypes.Name, id.ToString()) };

            var jwt = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: new SigningCredentials(_authOptions.GetSymmetricKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}