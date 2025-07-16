using AuthService.Entities;
using AuthService.Entities.Request;
using AuthService.Enums;

namespace AuthService.Mapping;

public static class UserMapper
{
    public static User ToUser(this UserRequest request)
    {
        return new User
        {
            Id = Guid.NewGuid().ToString(),
            Username = request.Username,
            PasswordHash = request.Password,
            Role =  nameof(Role.User)
        };
    }
}