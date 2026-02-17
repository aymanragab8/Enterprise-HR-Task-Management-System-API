using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication2.Enums;

namespace WebApplication2.Models.Entities
{
    public class Employee : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; } = DateTime.UtcNow;
        public string? JobTitle { get; set; }
        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public Salary? Salary { get; set; }
        public bool IsManager { get; set; } = false;
        public ICollection<LeaveRequest>? leaveRequests { get; set; }
        public ICollection<LeaveRequest>? ApprovedleaveRequests { get; set; }
        public ICollection<EmployeeTask>? AssignedTasks { get; set; }
        public ICollection<EmployeeTask>? CreatedTasks { get; set; }
        public string ApplicationUserId { get; set; }  
        public ApplicationUser ApplicationUser { get; set; }
    }


}
