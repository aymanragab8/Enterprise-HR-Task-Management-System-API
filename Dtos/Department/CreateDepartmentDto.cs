using System.ComponentModel.DataAnnotations;

public class CreateDepartmentDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Code { get; set; }

    public string? Description { get; set; }

    public int? ManagerId { get; set; }
}
