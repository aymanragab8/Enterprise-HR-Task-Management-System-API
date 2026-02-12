namespace WebApplication2.Models.Entities
{
    public class Salary
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Bonuses { get; set; } =0;
        public decimal Deductions { get; set; }=0;
        public decimal NetSalary => BasicSalary + Bonuses - Deductions;

    }
}
