namespace ProjectManager.Shared;

public class ProjectDto
{
    public enum StatusTypeDto {
        Active,
        Archived,
        Complete
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Goal { get; set; }
    public string Description { get; set; }
    public List<TaskItemDto> Tasks { get; set; } = new();
    public List<EmployeeDto> Assignees { get; set; } = new();
    public EmployeeDto? ProjectManager { get; set; }
    public List<MilestoneDto> Milestones { get; set; } = new();
    public StatusTypeDto Status { get; set; }
    public DateTime Created { get; init; } = DateTime.Now;
}