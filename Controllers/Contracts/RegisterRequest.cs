using System.ComponentModel.DataAnnotations;

namespace HouseRentalSystem.Controllers.Contracts;

public class RegisterRequest
{
    [Required]
    [MinLength(3)]
    public string FullName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = null!;
}
