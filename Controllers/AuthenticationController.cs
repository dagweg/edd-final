using System.Threading.Tasks;
using HouseRentalSystem.Models;
using HouseRentalSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentalSystem.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // issue a jwt token
        return Ok();
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
