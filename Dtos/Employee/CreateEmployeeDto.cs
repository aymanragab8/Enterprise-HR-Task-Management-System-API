using System.ComponentModel.DataAnnotations;

public class CreateEmployeeDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    public string Address { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string JobTitle { get; set; }

    public decimal Salary { get; set; }

    public int DepartmentId { get; set; }
}
