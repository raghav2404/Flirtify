using API.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Encoding = System.Text.Encoding;
using SymetricSecurityKey = Microsoft.IdentityModel.Tokens.SymmetricSecurityKey;
using API.Interfaces;
using API.DTOs;
namespace API.Services;

class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        // This method implements the logic to create a JWT token for the user.

       var tokenkey = config["TokenKey"] ?? throw new ArgumentNullException("TokenKey is not configured.");
       if(tokenkey.Length < 16)
       throw new ArgumentException("TokenKey must be at least 16 characters long.");

       var key = new SymetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey));

       var claims = new List<Claim>()
       {
        new Claim(ClaimTypes.NameIdentifier, user.UserName),

       };
       var tokenDescriptor = new SecurityTokenDescriptor
       {
              Subject = new ClaimsIdentity(claims),
              Expires = DateTime.Now.AddDays(7),
              SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
       };

         var tokenHandler = new JwtSecurityTokenHandler();
         var token = tokenHandler.CreateToken(tokenDescriptor);
         return tokenHandler.WriteToken(token);

    }
}