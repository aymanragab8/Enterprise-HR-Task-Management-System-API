namespace WebApplication2.Models.Entities
{
    public class Attendance
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

    }
}
