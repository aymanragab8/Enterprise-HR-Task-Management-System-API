using System.ComponentModel.DataAnnotations;
using WebApplication2.Enums;

namespace WebApplication2.Models.Entities
{
    public class EmployeeTask :BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public TaskState Status { get; set; } = TaskState.Pending;

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public DateTime DeadLine { get; set; }


        // 🔹 Foreign Keys
        public int? AssignedToEmployeeId { get; set; }
        public Employee? AssignedToEmployee { get; set; }

        public int? CreatedByEmployeeId { get; set; }
        public Employee? CreatedByEmployee { get; set; }
    }
}
