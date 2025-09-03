using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NZWalks.API.Repositories
{
    public class TokenRepo : ITokenRepositary
    {
        private readonly IConfiguration _configuration;

        public TokenRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            // -------------------------------
            // 1. Create Claims
            // -------------------------------
            // Claims are pieces of information about the user (email, roles, etc.)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email) // User's email claim
            };

            // Add all roles the user belongs to as claims (if i am tester and i want to be a user and admin so i can have more than one role)
            // JWT tokens store each role as a separate claim — not as a single combined string.
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // -------------------------------
            // 2. Create a Security Key
            // -------------------------------
            // Convert the secret key from appsettings.json into a symmetric key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            // -------------------------------
            // 3. Create Signing Credentials
            // -------------------------------
            // Use HMAC-SHA256 algorithm with the symmetric key to sign the token
            var credientials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Build the token with claims, issuer, audience, expiry, and signing credentials
            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],                   // Issuer from configuration
                _configuration["JWT:Audience"],                 // Audience from configuration
                claims,                                         // Add claims (email and roles)
                expires: DateTime.Now.AddMinutes(15),           // Token expiration time (15 minutes)
                signingCredentials: credientials                // Add the signing credentials
                );


            // -------------------------------
            // 5. Return Token as String
            // -------------------------------
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
