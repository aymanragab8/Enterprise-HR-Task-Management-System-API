using WebApplication2.Enums;

namespace WebApplication2.Dtos.Task
{
    public class UpdateEmployeeTaskDto
    {
        public TaskState? Status { get; set; }
        public TaskPriority? MyProperty { get; set; }
        public DateOnly? DeadLine { get; set; }
    }
}
