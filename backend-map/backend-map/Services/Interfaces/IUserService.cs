using backend_map.Models;

namespace backend_map.Services.Interfaces
{
    public interface IUserService
    {
        Task<AuthResult> Register(UserRegisterRequest request);
        Task<AuthResult> Login(UserLoginRequest request);
    }
}
