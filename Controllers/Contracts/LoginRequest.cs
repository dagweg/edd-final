using System.ComponentModel.DataAnnotations;

namespace HouseRentalSystem.Controllers.Contracts;

public class LoginRequest
{
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
