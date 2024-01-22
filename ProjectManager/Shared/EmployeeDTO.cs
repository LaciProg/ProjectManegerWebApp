namespace ProjectManager.Shared;

public class EmployeeDto
{
    public enum PermissionTypeDto {
        Admin,
        ProjectManager,
        Employee
    }
    public string EmailAddress { get; set; }
    public int Id { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public List<TaskItemDto> Tasks { get; set; }
    public PermissionTypeDto PermissionTypeLevel { get; set; }

}