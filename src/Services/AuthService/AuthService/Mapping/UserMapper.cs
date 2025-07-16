using AuthService.Entities;
using AuthService.Models;

namespace AuthService.Mapping;

public static class UserMapper
{
    public static User ToUser(this UserRequest request)
    {
        return new User
        {
            Id = Guid.NewGuid().ToString(),
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };
    }
}