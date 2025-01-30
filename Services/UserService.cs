using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using HouseRentalSystem.Exceptions;
using HouseRentalSystem.Models;
using HouseRentalSystem.Options;
using HouseRentalSystem.Services.MongoDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace HouseRentalSystem.Services;

public class UserService : IUserService
{
    private readonly IUserContext _userContext;
    private readonly IPasswordHasher<string> _passwordHasher;
    private readonly JwtOptions _jwtOptions;

    public UserService(
        IUserContext userContext,
        IPasswordHasher<string> passwordHasher,
        IOptions<JwtOptions> jwtOptions
    )
    {
        _userContext = userContext;
        _passwordHasher = passwordHasher;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task AddUserAsync(User newUser)
    {
        if (await _userContext.GetAsync(newUser.Id.ToString()) != null)
        {
            throw new AlreadyExistsException("User already exists.");
        }
        newUser.PasswordHash = _passwordHasher.HashPassword(newUser.Email, newUser.PasswordHash);
        await _userContext.CreateAsync(newUser);
    }

    public Task<bool> DeleteUserAsync(string id)
    {
        throw new NotImplementedException();
    }

    // returns a jwt token
    public async Task<string> LoginUserAsync(string email, string password)
    {
        var user = await _userContext.GetUserByEmailAsync(email);

        if (user is null)
        {
            throw new KeyNotFoundException("User not found. Please register.");
        }

        if (
            _passwordHasher.VerifyHashedPassword(user.Email, user.PasswordHash, password)
            == PasswordVerificationResult.Failed
        )
        {
            throw new UnauthorizedAccessException("Invalid password.");
        }

        var jwt = JwtTokenHelper.CreateToken(
            _jwtOptions.SecretKey,
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id), // to later access it for authorized request queries
                new Claim(JwtRegisteredClaimNames.Email, email), // same here
            },
            _jwtOptions.ExpiryMinutes
        );
        return jwt;
    }

    public Task<User> UpdateUserAsync(string id, User updatedUser)
    {
        throw new NotImplementedException();
    }
}
