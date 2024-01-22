using Duende.IdentityServer.Models;
using ProjectManager.Shared;

namespace ProjectManager.Server.Data.Models;

public static class Mapping
{
    public static ProjectDto ToDto(this Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Goal = project.Goal,
            Description = project.Description,
            Tasks = project.Tasks.Select(p=> p.ToDto()).ToList()!,
            ProjectManager = project.ProjectManager!.ToDto(),
            Status = project.Status.ToDto(),
            Assignees = project.Assignees.Select(e => e.ToDto()).ToList(),
            Milestones = project.Milestones.Select(m => m.ToDto()).ToList(),
            Created = project.Created
        };
    }
    
    public static Project ToModel(this ProjectDto projectDto)
    {
        return new Project
        {
            Id = projectDto.Id,
            Name = projectDto.Name,
            Goal = projectDto.Goal,
            Description = projectDto.Description,
            Tasks = projectDto.Tasks.Select(p=> p.ToModel()).ToList()!,
            ProjectManager = projectDto.ProjectManager.ToModel(),
            Status = projectDto.Status.ToModel(),
            Assignees = projectDto.Assignees.Select(e => e.ToModel()).ToList(),
            Milestones = projectDto.Milestones.Select(m => m.ToModel()).ToList(),
            Created = projectDto.Created
        };
    }
    
    public static TaskItemDto? ToDto(this TaskItem? task) {
        if (task == null) return null;

        return new TaskItemDto
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            Deadline = task.Deadline,
            Completed = task.Completed,
            Status = task.Status.ToDto(),
            Priority = task.Priority,
            Parent = task.Parent?.ToDto(),
            Assignees = task.Assignees.Select(e => e.ToDto()).ToList()
        };
    }
    
    public static List<TaskItemDto> ToDto(this List<TaskItem> taskListDto)
    {
        var r = new List<TaskItemDto>();
        foreach (var v in taskListDto)
        {
            var temp = v.ToDto();
            if (temp != null) r.Add(temp);
        }

        return r;
    }
    
    public static TaskItem ToModel(this TaskItemDto taskDto)
    {
        return new TaskItem
        {
            Id = taskDto.Id,
            Name = taskDto.Name,
            Description = taskDto.Description,
            Deadline = taskDto.Deadline,
            Completed = taskDto.Completed,
            Status = taskDto.Status.ToModel(),
            Priority = taskDto.Priority,
            Parent = taskDto.Parent?.ToModel(),
            Assignees = taskDto.Assignees.Select(e => e.ToModel()).ToList()
        };
    }

    public static List<TaskItem> ToModel(this List<TaskItemDto> taskListDto)
    {
        var r = new List<TaskItem>();
        foreach (var v in taskListDto)
        {
            var temp = v.ToModel();
            r.Add(temp);
        }

        return r;
    }
    
    public static MilestoneDto ToDto(this Milestone milestone)
    {
        return new MilestoneDto
        {
            Id = milestone.Id,
            Name = milestone.Name,
            Description = milestone.Description,
            Deadline = milestone.Deadline,
        };
    }
    
    public static Milestone ToModel(this MilestoneDto milestoneDto)
    {
        return new Milestone
        {
            Id = milestoneDto.Id,
            Name = milestoneDto.Name,
            Description = milestoneDto.Description,
            Deadline = milestoneDto.Deadline,
        };
    }
    
    public static List<Milestone> ToModel(this List<MilestoneDto> milestoneDtos)
    {
        var r = new List<Milestone>();
        foreach (var v in milestoneDtos)
        {
            var temp = v.ToModel();
            r.Add(temp);
        }

        return r;
    }
    
    public static EmployeeDto ToDto(this Employee employee)
    {
        return new EmployeeDto
        {
            EmailAddress = employee.EmailAddress,
            Id = employee.Id,
            Name = employee.Name,
            PermissionTypeLevel = employee.PermissionLevel.ToDto(),
            Password = employee.Password,
            BirthDate = employee.BirthDate,
        };
    }
    
    public static Employee ToModel(this EmployeeDto employeeDto)
    {
        return new Employee
        {
            EmailAddress = employeeDto.EmailAddress,
            Name = employeeDto.Name,
            Id = employeeDto.Id,
            PermissionLevel = employeeDto.PermissionTypeLevel.ToModel(),
            Password = employeeDto.Password,
            BirthDate = employeeDto.BirthDate,
        };
    }
    
    public static List<Employee> ToModel(this List<EmployeeDto> employeeDtos)
    {
        var r = new List<Employee>();
        foreach (var v in employeeDtos)
        {
            var temp = v.ToModel();
            r.Add(temp);
        }

        return r;
    }
    
    public static CostCenterDto ToDto(this CostCenter costCenter)
    {
        return new CostCenterDto
        {
            Id = costCenter.Id,
            Name = costCenter.Name,
            Manager = costCenter.Manager.ToDto(),
            Deadline = costCenter.Deadline,
            Project = costCenter.Project.ToDto(),
        };
    }
    
    public static CostCenter ToModel(this CostCenterDto costCenterDto)
    {
        return new CostCenter
        {
            Id = costCenterDto.Id,
            Name = costCenterDto.Name,
            Manager = costCenterDto.Manager.ToModel(),
            Deadline = costCenterDto.Deadline,
            Project = costCenterDto.Project.ToModel()
        };
    }

    private static ProjectDto.StatusTypeDto ToDto(this Project.StatusType statusType) {
        return statusType switch {
            Project.StatusType.Archived => ProjectDto.StatusTypeDto.Archived,
            Project.StatusType.Complete => ProjectDto.StatusTypeDto.Complete,
            _ => ProjectDto.StatusTypeDto.Active
        };
    }

    private static Project.StatusType ToModel(this ProjectDto.StatusTypeDto statusTypeDto) {
        return statusTypeDto switch {
            ProjectDto.StatusTypeDto.Archived => Project.StatusType.Archived,
            ProjectDto.StatusTypeDto.Complete => Project.StatusType.Complete,
            _ => Project.StatusType.Active
        };
    }

    private static EmployeeDto.PermissionTypeDto ToDto(this Employee.PermissionType permissionType) {
        return permissionType switch {
            Employee.PermissionType.Admin => EmployeeDto.PermissionTypeDto.Admin,
            Employee.PermissionType.ProjectManager => EmployeeDto.PermissionTypeDto.ProjectManager,
            _ => EmployeeDto.PermissionTypeDto.Employee
        };
    }

    private static Employee.PermissionType ToModel(this EmployeeDto.PermissionTypeDto permissionTypeDto) {
        return permissionTypeDto switch {
            EmployeeDto.PermissionTypeDto.Admin => Employee.PermissionType.Admin,
            EmployeeDto.PermissionTypeDto.ProjectManager => Employee.PermissionType.ProjectManager,
            _ => Employee.PermissionType.Employee
        };
    }

}