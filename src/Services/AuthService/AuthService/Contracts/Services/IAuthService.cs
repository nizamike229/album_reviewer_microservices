using AuthService.Entities;

namespace AuthService.Contracts.Services;

public interface IAuthService
{
    Task<string> Login(User user);
    Task<string> Register(User user);
}