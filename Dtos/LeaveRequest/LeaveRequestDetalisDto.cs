namespace WebApplication2.Dtos.LeaveRequest
{
    public class LeaveRequestDetalisDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }

}
