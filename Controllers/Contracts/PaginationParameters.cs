using System.ComponentModel.DataAnnotations;

namespace HouseRentalSystem.Controllers.Contracts;

public class PaginationParameters
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, int.MaxValue)]
    public int PageSize { get; set; } = 10;
}
