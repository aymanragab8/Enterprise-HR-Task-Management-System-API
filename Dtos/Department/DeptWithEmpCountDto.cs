using System.Text.Json.Serialization;

namespace WebApplication2.Dtos.Department
{
    public class DeptWithEmpCountDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public int EmployeeCount { get; set; }
    }
}
