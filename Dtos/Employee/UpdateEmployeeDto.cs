using WebApplication2.Enums;
using WebApplication2.Models;

public class UpdateEmployeeDto
{
    public string? Address { get; set; }
    public string? JobTitle { get; set; }
    public string? PhoneNumber { get; set; }
    public int? DepartmentId { get; set; }
    public EmployeeStatus? Status { get; set; }
}
