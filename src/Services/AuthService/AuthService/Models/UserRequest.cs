using System.ComponentModel.DataAnnotations;

namespace AuthService.Models;

public class UserRequest
{
    [MinLength(8)]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "The field cannot consist only of spaces")]
    public required string Username { get; set; }

    [MinLength(8)]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "The field cannot consist only of spaces")]
    public required string Password { get; set; }
}