using AuthService.Contracts.Services;
using AuthService.Entities.Request;
using AuthService.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;

    public AuthController(IAuthService authService, IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
    }

    [HttpPost]
    [ActionName("login")]
    public async Task<ActionResult<string>> Login([FromBody] UserRequest request)
    {
        var token = await _authService.Login(request.ToUser());
        Response.Cookies.Append("access_token", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]))
        });
        return Ok(token);
    }

    [HttpPost]
    [ActionName("register")]
    public async Task<ActionResult<string>> Register([FromBody] UserRequest request)
    {
        return Ok(new { Message = await _authService.Register(request.ToUser()) });
    }

    [Authorize]
    [HttpPost]
    [ActionName("logout")]
    public ActionResult<string> Logout()
    {
        Response.Cookies.Delete("access_token");
        return Ok(new { Message = "Logout successful" });
    }
}