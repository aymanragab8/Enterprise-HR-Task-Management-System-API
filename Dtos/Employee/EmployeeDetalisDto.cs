using WebApplication2.Enums;

namespace WebApplication2.Dtos.Employee
{
    public class EmployeeDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string DepartmentName { get; set; }
        public EmployeeStatus Status { get; set; }
        public decimal? BasicSalary { get; set; }
    }

}
