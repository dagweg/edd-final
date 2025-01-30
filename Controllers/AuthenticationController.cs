using System.Security.Claims;
using System.Threading.Tasks;
using HouseRentalSystem.Controllers.Contracts;
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

    /// <summary>
    /// Gives a JWT token to the user if the credentials are correct. User then can use this token to authenticate themselves by sending it in the Authorization header.
    /// </summary>
    /// <param name="request"></param>
    /// <response code="200">Returns the JWT token and the expiration time.</response>
    /// <response code="400">Bad request. The request is invalid.</response>
    /// <response code="500">Internal server error. Something went wrong on the server.</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var jwt = await _userService.LoginUserAsync(request.Email, request.Password);
        return Ok(
            new { Token = jwt, ExpiresIn = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes) }
        );
    }

    /// <summary>
    /// Registers a new user into the system. The user can then login with the credentials provided.
    /// </summary>
    /// <param name="request"></param>
    /// <response code="201">The user was successfully registered.</response>
    /// <response code="400">Bad request. The request is invalid.</response>
    /// <response code="500">Internal server error. Something went wrong on the server.</response>
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
