using AuthService.Entities;

namespace AuthService.Contracts.Repositories;

public interface IAuthRepository
{
    Task<string> Register(User user);
    Task<string> Login(User request);
}