using System.ComponentModel.DataAnnotations;

public class CreateLeaveRequestDto
{
    [Required]
    public DateTime FromDate { get; set; }

    [Required]
    public DateTime ToDate { get; set; }

    [Required]
    public string Reason { get; set; }
}
