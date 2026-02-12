using WebApplication2.Enums;
using WebApplication2.Models;

public class UpdateEmployeeDto
{
    public string? Address { get; set; }

    public string? JobTitle { get; set; }

    public decimal? Salary { get; set; }

    public EmployeeStatus? Status { get; set; }
}
