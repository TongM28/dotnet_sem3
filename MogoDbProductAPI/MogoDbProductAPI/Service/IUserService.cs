using MogoDbProductAPI.Domain.Model;

public interface IUserService
{
    Task<User> Register(User user, string password);
    Task<string?> Login(string email, string password);
}
