using WebApplication2.Enums;

namespace WebApplication2.Dtos.Task
{
    public class CreateEmployeeTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime DeadLine { get; set; }
        public int AssignedToEmployeeId { get; set; }

    }
}
