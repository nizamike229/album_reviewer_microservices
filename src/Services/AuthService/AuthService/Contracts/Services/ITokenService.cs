using AuthService.Entities;

namespace AuthService.Contracts.Services;

public interface ITokenService
{
    string GenerateJwtToken(User user);
}