using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models.Entities
{
    public class Department : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }   // HR, IT, FIN
        public string? Description { get; set; }
        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }
}
