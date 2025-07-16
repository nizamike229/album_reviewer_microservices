using AuthService.Contracts.Repositories;
using AuthService.Contracts.Services;
using AuthService.Entities;

namespace AuthService.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repo;

    public AuthService(IAuthRepository repo)
    {
        _repo = repo;
    }

    public async Task<string> Login(User user)
    {
        return await _repo.Login(user);
    }

    public async Task<string> Register(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.PasswordHash))
            throw new Exception("Username and/or password are required");

        if (user.PasswordHash.Length < 8 || user.PasswordHash.Length > 30)
            throw new Exception("Password must be between 8 and 30 characters");

        if (user.Username.Length < 8 || user.Username.Length > 30)
            throw new Exception("Username must be between 8 and 30 characters");

        return await _repo.Register(user);
    }
}