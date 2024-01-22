using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Server.Data;
using ProjectManager.Server.Data.Models;
using ProjectManager.Shared;

namespace ProjectManager.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public ProjectController(ApplicationDbContext context)
    {
        _context = context;
    }

    //TODO: this doesn't work for some reason
    //Create/Edit
    [HttpPost("SaveProject")]
    public async Task<ActionResult<ProjectDto>> SaveNewProject([FromBody] ProjectDto projectDto)
    {
        var emp = await _context.Employees
            .Include(e => e.Tasks)
            .FirstOrDefaultAsync(e => User.Identity != null && e.EmailAddress == User.Identity.Name);
        /*if (emp?.PermissionLevel is not (Employee.PermissionType.ProjectManager or Employee.PermissionType.Admin))
            return Unauthorized();*/
        if (projectDto.Id == 0)
        {
            await _context.Projects.AddAsync(new Project
            {
                Name = projectDto.Name,
                Goal = projectDto.Goal,
                Status = (Project.StatusType)projectDto.Status,
                Tasks = projectDto.Tasks.ToModel(),
                Description = projectDto.Description,
                Assignees = projectDto.Assignees.ToModel(),
                Milestones = projectDto.Milestones.ToModel(),
                ProjectManager = await _context.Employees
                    .Include(e => e.Tasks)
                    .FirstOrDefaultAsync(e => projectDto.ProjectManager != null && e.Id == projectDto.ProjectManager.Id)
                
            });
        }
        else
        {
            var projectInDb = await _context.Projects
                .Include(p => p.Assignees)
                .Include(p => p.Milestones)
                .Include(p => p.ProjectManager)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p=> p.Id == projectDto.Id);
            if (projectInDb == null)
            {
                //ProblemDetails problemDetails = new ProblemDetails
                throw new KeyNotFoundException();
            }

            projectInDb.Name = projectDto.Name;
            projectInDb.Goal = projectDto.Goal;
            projectInDb.Description = projectDto.Description;
            projectInDb.Status = (Project.StatusType)projectDto.Status;
            projectInDb.Tasks = projectDto.Tasks.ToModel();
            projectInDb.Assignees = projectDto.Assignees.ToModel();
            projectInDb.Milestones = projectDto.Milestones.ToModel();
            if (projectDto.ProjectManager != null)
                projectInDb.ProjectManager = await _context.Employees
                    .Include(e => e.Tasks)
                    .FirstOrDefaultAsync(e => e.Id == projectDto.ProjectManager.Id);
        }
        await _context.SaveChangesAsync();
        return projectDto;

    }

    //Add task to project
    [HttpPost("AddTaskToProject/{projectId:int}/{taskId:int}")]
    public async Task<ActionResult<ProjectDto>> AddTaskToProject(int projectId, int taskId)
    {
		var emp = await _context.Employees
            .Include(e => e.Tasks)
            .Where(e => User.Identity != null && e.EmailAddress == User.Identity.Name).FirstOrDefaultAsync();
		if (!(emp?.PermissionLevel == Employee.PermissionType.ProjectManager || emp?.PermissionLevel == Employee.PermissionType.Admin))
			return Unauthorized();

		var projectInDb = await _context.Projects
            .Include(p => p.Assignees)
            .Include(p => p.Milestones)
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p=> p.Id ==projectId);
        if (projectInDb == null)
        {
            throw new KeyNotFoundException();
        }
        var taskInDb = await _context.Tasks
            .Include(t => t.Assignees)
            .Include(t => t.Parent)
            .FirstOrDefaultAsync(t=> t.Id ==taskId);
        if (taskInDb == null)
        {
            throw new KeyNotFoundException();
        }
        projectInDb.Tasks.Add(taskInDb);
        await _context.SaveChangesAsync();
        return projectInDb.ToDto();
    }
    
    //Add milestone to project
    [HttpPost("AddMilestoneToProject/{projectId:int}/{milestoneId:int}")]
    public async Task<ActionResult<ProjectDto>> AddMilestoneToProject(int projectId, int milestoneId)
    {
		var emp = await _context.Employees
            .Include(e => e.Tasks)
            .Where(e => User.Identity != null && e.EmailAddress == User.Identity.Name).FirstOrDefaultAsync();
		if (!(emp?.PermissionLevel == Employee.PermissionType.ProjectManager || emp?.PermissionLevel == Employee.PermissionType.Admin))
			return Unauthorized();

		var projectInDb = await _context.Projects
            .Include(p => p.Assignees)
            .Include(p => p.Milestones)
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p=> p.Id ==projectId);
        if (projectInDb == null)
        {
            throw new KeyNotFoundException();
        }
        var milestoneInDb = await _context.Milestones.FindAsync(milestoneId);
        if (milestoneInDb == null)
        {
            throw new KeyNotFoundException();
        }
        projectInDb.Milestones.Add(milestoneInDb);
        await _context.SaveChangesAsync();
        return projectInDb.ToDto();
    }
    
    //Add employee to project
    [HttpPost("AddEmployeeToProject/{projectId:int}/{employeeId:int}")]
    public async Task<ActionResult<ProjectDto>> AddEmployeeToProject(int projectId, int employeeId)
    {
		var emp = await _context.Employees
            .Include(e => e.Tasks)
            .Where(e => User.Identity != null && e.EmailAddress == User.Identity.Name).FirstOrDefaultAsync();
		if (!(emp?.PermissionLevel == Employee.PermissionType.ProjectManager || emp?.PermissionLevel == Employee.PermissionType.Admin))
			return Unauthorized();

		var projectInDb = await _context.Projects
            .Include(p => p.Assignees)
            .Include(p => p.ProjectManager)
            .Include(p => p.Milestones)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == projectId);
        if (projectInDb == null)
        {
            throw new KeyNotFoundException();
        }
        var employeeInDb = await _context.Employees
            .Include(e => e.Tasks)
            .FirstOrDefaultAsync(e=> e.Id == employeeId);
        if (employeeInDb == null)
        {
            throw new KeyNotFoundException();
        }
        projectInDb.Assignees.Add(employeeInDb);
        await _context.SaveChangesAsync();
        return projectInDb.ToDto();
    }
    
    //Get
    [HttpGet("GetProject/{id:int}")]
    public async Task<ActionResult<ProjectDto>> GetProject(int id)
    {
        var result = await _context.Projects
            .Include(p =>p.Assignees)
            .Include(p => p.Milestones)
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (result == null)
        {
            throw new KeyNotFoundException();
        } 
        if (result.Assignees.All(p => p.EmailAddress != User.Identity?.Name)) return Unauthorized();
        return result.ToDto();
    }
    
    //Get all
    [HttpGet("GetAllProjects")]
    public async Task<ActionResult<List<ProjectDto>>> GetProjects()
    {
        var projects = await _context.Projects.Include(p => p.Tasks)
            .Include(p => p.Assignees)
            .Include(p => p.ProjectManager)
            .Include(p => p.Milestones)
            .Where(p => p.Assignees.Any(e => User.Identity != null && e.EmailAddress == User.Identity.Name)).ToListAsync();
        
        return projects.Select(p => p.ToDto()).ToList();
    }
    
    //Get tasks
    [HttpGet("GetTasksForProject({projectId:int}")]
    public async Task<ActionResult<List<TaskItemDto>>> GetTasksForProject(int projectId)
    {
        var tasksInProjectInDb = await _context.Projects
            .Include(p => p.Assignees)
            .Include(p => p.Milestones)
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .Where(p => p.Id == projectId && p.Assignees.Any(e => User.Identity != null && e.EmailAddress == User.Identity.Name))
            .SelectMany(p => p.Tasks)
           .ToListAsync();
        if (tasksInProjectInDb.Count == 0)
        {
            throw new KeyNotFoundException();
        }
        return tasksInProjectInDb.Select(t => t.ToDto()).ToList()!;
    }
    
    //Get task
    [HttpGet("GetTaskForProject/{projectId:int}/{taskId:int}")]
    public async Task<ActionResult<TaskItemDto>> GetTaskForProject(int projectId, int taskId)
    {
        var projectInDb = await _context.Projects                
            .Include(p => p.Assignees)
            .Include(p => p.Milestones)
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p=> p.Id ==projectId);
        if (projectInDb == null)
        {
            throw new KeyNotFoundException();
        }
        var taskInDb = await _context.Tasks
            .Include(t => t.Assignees)
            .Include(t => t.Parent)
            .FirstOrDefaultAsync(t=> t.Id == taskId);
        if (taskInDb == null)
        {
            throw new KeyNotFoundException();
        }
        if (taskInDb.Assignees.All(p => p.EmailAddress != User.Identity?.Name)) return Unauthorized();
        return taskInDb.ToDto()!;
    }
    
    //Get milestones
    [HttpGet("GetMilestonesForProject/{projectId:int}")]
    public async Task<ActionResult<List<MilestoneDto>>> GetMilestonesForProject(int projectId)
    {
        var milestonesInProjectInDb = await _context.Projects
            .Include(p => p.Assignees)
            .Include(p => p.Milestones)
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .Where(p => p.Id == projectId)
            .SelectMany(p => p.Milestones)
            .ToListAsync();
        if (milestonesInProjectInDb.Count == 0)
        {
            throw new KeyNotFoundException();
        }
        return milestonesInProjectInDb.Select(m => m.ToDto()).ToList();
    }
    
    //Get milestone
    [HttpGet("GetMilestoneForProject/{projectId:int}/{milestoneId:int}")]
    public async Task<ActionResult<MilestoneDto>> GetMilestoneForProject(int projectId, int milestoneId)
    {
        var projectInDb = await _context.Projects
            .Include(p => p.Assignees)
            .Include(p => p.Milestones)
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p=> p.Id ==projectId);
        if (projectInDb == null)
        {
            throw new KeyNotFoundException();
        }
        var milestoneInDb = await _context.Milestones.FindAsync(milestoneId);
        if (milestoneInDb == null)
        {
            throw new KeyNotFoundException();
        }
        return milestoneInDb.ToDto();
    }
    
    //Get assignees
    [HttpGet("GetAssigneesForProject/{projectId:int}")]
    public async Task<ActionResult<List<EmployeeDto>>> GetAssigneesForProject(int projectId)
    {
		var emp = await _context.Employees
            .Include(e => e.Tasks)
            .Where(e => User.Identity != null && e.EmailAddress == User.Identity.Name).FirstOrDefaultAsync();
		if (!(emp?.PermissionLevel == Employee.PermissionType.ProjectManager || emp?.PermissionLevel == Employee.PermissionType.Admin))
			return Unauthorized();

		var assigneesInProjectInDb = await _context.Projects
            .Include(p => p.Assignees)
            .Include(p => p.Milestones)
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .Where(p => p.Id == projectId)
            .SelectMany(p => p.Assignees)
            .ToListAsync();
        if (assigneesInProjectInDb.Count == 0)
        {
            throw new KeyNotFoundException();
        }
        return assigneesInProjectInDb.Select(a => a.ToDto()).ToList();
    }
    
    //Get assignee
    [HttpGet("GetAssigneeForProject/{projectId:int}/{employeeId:int}")]
    public async Task<ActionResult<EmployeeDto>> GetAssigneeForProject(int projectId, int employeeId)
    {
		var emp = await _context.Employees
            .Include(e => e.Tasks)
            .Where(e => User.Identity != null && e.EmailAddress == User.Identity.Name).FirstOrDefaultAsync();
		if (!(emp?.PermissionLevel == Employee.PermissionType.ProjectManager || emp?.PermissionLevel == Employee.PermissionType.Admin))
			return Unauthorized();

		var projectInDb = await _context.Projects
            .Include(p => p.Assignees)
            .Include(p => p.Milestones)
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p=> p.Id ==projectId);
        if (projectInDb == null)
        {
            throw new KeyNotFoundException();
        }
        var assigneeInDb = await _context.Employees
            .Include(e => e.Tasks)
            .FirstOrDefaultAsync(e => e.Id == employeeId);
        if (assigneeInDb == null)
        {
            throw new KeyNotFoundException();
        }
        return assigneeInDb.ToDto();
    }
    
    //Get project manager
    [HttpGet("GetProjectManagerForProject/{projectId:int}/{projectManagerId:int}")]
    public async Task<ActionResult<EmployeeDto>> GetProjectManagerForProject(int projectId, int projectManagerId)
    {
        var projectInDb = await _context.Projects
            .Include(p => p.Assignees)
            .Include(p => p.Milestones)
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p=> p.Id ==projectId);
        if (projectInDb == null)
        {
            throw new KeyNotFoundException();
        }
        var projectManagerInDb = await _context.Employees
            .Include(e => e.Tasks)
            .FirstOrDefaultAsync(e => e.Id ==projectManagerId);
        if (projectManagerInDb == null)
        {
            throw new KeyNotFoundException();
        }

        return projectManagerInDb.ToDto();
    }

    //Get active projects for a user
    [HttpGet("GetActiveProjectsFor/{id:int}")]
    public async Task<ActionResult<List<ProjectDto>>> GetActiveProjectsFor(int id)
    {
		var employee = await _context.Employees
            .Include(e => e.Tasks)
            .FirstOrDefaultAsync(e=> e.Id ==id);
		if (employee == null)
		{
			throw new KeyNotFoundException();
		}

        var projects = await _context.Projects
            .Include(p => p.Assignees)
            .Include(p => p.Milestones)
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .Where(p => p.Assignees.Contains(employee) && p.Status == Project.StatusType.Active).ToListAsync();
        return projects.Select(p => p.ToDto()).ToList();

    }
    
    //Get active projects for a user
    [HttpGet("GetActiveTasksFor/{id:int}")]
    public async Task<ActionResult<List<TaskItemDto>>> GetActiveTasksFor(int id)
    {

    		var employee = await _context.Employees
                .Include(e => e.Tasks)
                .FirstOrDefaultAsync(e => e.Id ==id);
        if (employee == null)
        {
            throw new KeyNotFoundException();
        }

        var projects = await _context.Projects
            .Include(p => p.Assignees)
            .Include(p => p.Milestones)
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .Where(p => p.Assignees.Contains(employee) && p.Status == Project.StatusType.Active)
            .Select(p => p.ToDto()).ToListAsync();

        var tasks = new List<TaskItemDto>();

        foreach (var p in projects)
        {
            tasks.AddRange(p.Tasks.FindAll(t => t.Status == ProjectDto.StatusTypeDto.Active)); 
        }

        return tasks;
    }
    
    
    
    //Get archived projects for a user
    [HttpGet("GetArchivedProjectsFor/{id:int}")]
    public async Task<ActionResult<List<ProjectDto>>> GetArchivedProjectsFor(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.Tasks)
            .FirstOrDefaultAsync(e=> e.Id ==id);
        if (employee == null)
        {
            throw new KeyNotFoundException();
        }

        var projects = await _context.Projects
            .Include(p => p.Assignees)
            .Include(p => p.Milestones)
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .Where(p => p.Assignees.Contains(employee) && p.Status == Project.StatusType.Archived).ToListAsync();
        return projects.Select(p => p.ToDto()).ToList();

    }
    
    //Get active projects for a user
    [HttpGet("GetArchivedTasksFor/{id:int}")]
    public async Task<ActionResult<List<TaskItemDto>>> GetArchivedTasksFor(int id)
    {
        
        var employee = await _context.Employees
            .Include(e => e.Tasks)
            .FirstOrDefaultAsync(e=> e.Id ==id);
        if (employee == null)
        {
            throw new KeyNotFoundException();
        }

        var projects = await _context.Projects
            .Include(p => p.Assignees)
            .Include(p => p.Milestones)
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .Where(p => p.Assignees.Contains(employee) && p.Status == Project.StatusType.Archived)
            .Select(p => p.ToDto()).ToListAsync();

        var tasks = new List<TaskItemDto>();

        foreach (var p in projects)
        {
            tasks.AddRange(p.Tasks.FindAll(t => t.Status == ProjectDto.StatusTypeDto.Active)); 
        }

        return tasks;
    }
    
    //Delete
    [HttpDelete("DeleteProject/{id:int}")]
    public async Task<ActionResult> DeleteProject(int id)
    {
		var emp = await _context.Employees
            .Include(e => e.Tasks)
            .Where(e => User.Identity != null && e.EmailAddress == User.Identity.Name).FirstOrDefaultAsync();
		if (!(emp?.PermissionLevel == Employee.PermissionType.ProjectManager || emp?.PermissionLevel == Employee.PermissionType.Admin))
			return Unauthorized();

		var result = await _context.Projects
            .Include(p => p.Assignees)
            .Include(p => p.Milestones)
            .Include(p => p.ProjectManager)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p=> p.Id ==id);
        if (result == null)
        {
            throw new KeyNotFoundException();
        }
        
        _context.Projects.Remove(result);
        await _context.SaveChangesAsync();
        return Ok();
    }
}