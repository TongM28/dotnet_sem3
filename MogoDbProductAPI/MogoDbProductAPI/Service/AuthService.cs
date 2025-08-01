using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MogoDbProductAPI.Domain.Model;
using MogoDbProductAPI.Models;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MogoDbProductAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration, IMongoDatabase database)
        {
            _configuration = configuration;
            _userCollection = database.GetCollection<User>("Users");
        }

        public string Login(LoginRequest request)
        {
            var user = _userCollection.Find(u => u.Email == request.Email && u.PasswordHash == request.Password).FirstOrDefault();
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role ?? "User")
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:DurationInMinutes"]!)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
