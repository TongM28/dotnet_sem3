using MogoDbProductAPI.Data;
using MogoDbProductAPI.Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MogoDbProductAPI.Domain.Contracts;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _users;
    private readonly IConfiguration _config;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(IOptions<MongoDBSettings> dbSettings, IConfiguration config)
    {
        var client = new MongoClient(dbSettings.Value.ConnectionURI);
        var database = client.GetDatabase(dbSettings.Value.DatabaseName);
        _users = database.GetCollection<User>("Users");

        _passwordHasher = new PasswordHasher<User>();
        _config = config;
    }

    public async Task<User> Register(User user, string password)
    {
        user.PasswordHash = _passwordHasher.HashPassword(user, password);
        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task<string?> Login(string email, string password)
    {
        var user = await _users.Find(x => x.Email == email).FirstOrDefaultAsync();
        if (user == null) return null;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed) return null;

        // Create JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
