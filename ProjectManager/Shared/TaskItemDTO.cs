namespace ProjectManager.Shared;

public class TaskItemDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public uint Priority { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime? Completed { get; set; }
    public string Name { get; set; }
    public TaskItemDto? Parent { get; set; } = null;
    public ProjectDto.StatusTypeDto Status { get; set; }
    public List<EmployeeDto> Assignees { get; set; } = new();

}