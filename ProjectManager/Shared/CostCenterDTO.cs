namespace ProjectManager.Shared;

public class CostCenterDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public EmployeeDto Manager { get; set; }
    public DateTime Deadline { get; set; }
    public ProjectDto Project { get; set;}
}