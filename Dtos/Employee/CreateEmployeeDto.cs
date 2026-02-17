using System.ComponentModel.DataAnnotations;

public class CreateEmployeeDto
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Address { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string JobTitle { get; set; }



}
