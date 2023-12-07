namespace Workshop.Example.Api.Models.Requests
{
    public class CreateTaskRequest
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Assignee { get; set; }
        public DateTime? AssignmentDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int? CompletionPercentage { get; set; }
        public string? Notes { get; set; }
    }
}
