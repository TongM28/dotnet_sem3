using MogoDbProductAPI.Models;

namespace MogoDbProductAPI.Service
{
    public interface IAuthService
    {
        string Login(LoginRequest request);
    }
}
