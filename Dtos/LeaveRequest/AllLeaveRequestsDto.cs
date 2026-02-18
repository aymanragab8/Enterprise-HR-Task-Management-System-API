namespace WebApplication2.Dtos.LeaveRequest
{
    public class AllLeaveRequestsDto
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; }
        public int? ApprovedByEmployeeId { get; set; }
    }
}
