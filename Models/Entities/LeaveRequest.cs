using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication2.Enums;

namespace WebApplication2.Models.Entities
{
    public class LeaveRequest : BaseEntity
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; }
        public LeaveRequestStatus Status { get; set; } = LeaveRequestStatus.Pending;
        public int? ApprovedByEmployeeId { get; set; }
        public Employee? ApprovedByEmployee { get; set; }
        public int TotalDays => (ToDate - FromDate).Days + 1;
    }
}
