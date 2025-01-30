using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenHelper
{
    public static string CreateToken(
        string key,
        string issuer,
        string audience,
        IEnumerable<Claim> claims,
        int expiryMinutes
    )
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Get the user ID from the JWT token.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public static string GetUserId(ClaimsPrincipal user)
    {
        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        return userIdClaim is null ? throw new UnauthorizedAccessException() : userIdClaim.Value;
    }
}
