using Microsoft.AspNetCore.Mvc;
using ProjectManager.Server.Data.Models;
using ProjectManager.Server.Data;
using ProjectManager.Shared;
using Microsoft.EntityFrameworkCore;

namespace ProjectManager.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TaskController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		public TaskController(ApplicationDbContext context)
		{
			_context = context;
		}


		//Create/Edit
		[HttpPost("SaveTask")]
		public async Task<ActionResult<TaskItemDto>> SaveTask([FromBody] TaskItemDto taskDto)
		{
			if (taskDto.Id == 0) //new
			{
				await _context.Tasks.AddAsync(new TaskItem
				{
					Description = taskDto.Description,
					Priority = taskDto.Priority,
					Deadline = taskDto.Deadline,
					Name = taskDto.Name,
					Parent = taskDto.ToModel().Parent,
					Status = taskDto.ToModel().Status,
					Assignees = taskDto.ToModel().Assignees,
					//id generálás?
				});
			}
			else //edit
			{
				var taskInDb = await _context.Tasks
					.Include(t => t.Assignees)
					.Include(t => t.Parent)
					.FirstOrDefaultAsync(t=> t.Id ==taskDto.Id);
				if (taskInDb == null)
				{
					throw new KeyNotFoundException();
				}
				taskInDb.Description = taskDto.Description;
				taskInDb.Priority = taskDto.Priority;
				taskInDb.Deadline = taskDto.Deadline;
				taskInDb.Name = taskDto.Name;
				taskInDb.Parent = taskDto.ToModel().Parent;
				taskInDb.Status = taskDto.ToModel().Status;
				taskInDb.Assignees = taskDto.ToModel().Assignees;
			}
			await _context.SaveChangesAsync();
			return taskDto;
		}

		//Get one task by id
		[HttpGet("/GetTask/{id:int}")]
		public async Task<ActionResult<TaskItemDto>> GetTask(int id)
		{
			var res = await _context.Tasks
				.Include(t => t.Assignees)
				.Include(t => t.Parent)
				.FirstOrDefaultAsync(t => t.Id == id);
			if (res == null)
			{
				throw new KeyNotFoundException();
			}
			return res.ToDto()!;
		}

		//Get all tasks
		[HttpGet("GetAllTasks")]
		public async Task<ActionResult<List<TaskItemDto>>> GetTasks()
		{
			var results = await _context.Tasks
				.Include(t => t.Assignees)
				.Include(t => t.Parent)
				.Where(p => p.Assignees.Any(e => User.Identity != null && e.EmailAddress == User.Identity.Name))
				.ToListAsync();
			return results.Select(e => e.ToDto()).ToList()!;
		}

		//Get the employees working on this task
		[HttpGet("GetAllEmployees/{taskId:int}")]
		public async Task<ActionResult<List<EmployeeDto>>> GetEmployees(int taskId)
		{
			var res = await _context.Tasks
				.Include(t => t.Assignees)
				.Include(t => t.Parent)
				.FirstOrDefaultAsync(t => t.Id ==taskId);
			if(res == null)
			{
				throw new KeyNotFoundException();
			}

			return res.Assignees.Select(a => a.ToDto()).ToList();
		}

		//Delete
		[HttpDelete("DeleteTask/{id:int}")]
		public async Task<ActionResult> DeleteTask(int id)
		{
			var res = await _context.Tasks
				.Include(t => t.Assignees)
				.Include(t => t.Parent)
				.FirstOrDefaultAsync(t => t.Id == id);
			if (res == null)
			{
				throw new KeyNotFoundException();
			}
			_context.Tasks.Remove(res);
			await _context.SaveChangesAsync();
			return Ok();
		}
	}
}
