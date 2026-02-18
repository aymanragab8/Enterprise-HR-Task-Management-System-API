namespace WebApplication2.Dtos.Task
{
    public class TaskDetalisDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public DateTime DeadLine { get; set; }
        public string AssignedTo { get; set; }
    }
}
