using System.Threading.Tasks;
using HouseRentalSystem.Models;
using HouseRentalSystem.Options;
using HouseRentalSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HouseRentalSystem.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly JwtOptions _jwtOptions;

    public AuthenticationController(IUserService userService, IOptions<JwtOptions> jwtOptions)
    {
        _userService = userService;
        _jwtOptions = jwtOptions.Value;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.LoginUserAsync(request.Email, request.Password);
        return Ok(
            new { Token = user, ExpiresIn = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes) }
        );
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await _userService.AddUserAsync(
            new User
            {
                Email = request.Email,
                PasswordHash = request.Password,
                Name = request.FullName,
            }
        );

        return Created();
    }
}
