using Microsoft.AspNetCore.Mvc;
using ProjectManager.Server.Data.Models;
using ProjectManager.Server.Data;
using ProjectManager.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Operations;

namespace ProjectManager.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		public EmployeeController(ApplicationDbContext context)
		{
			_context = context;
		}


		//Create/Edit
		[HttpPost("SaveEmployee")]
		public async Task<ActionResult<EmployeeDto>> SaveEmployee([FromBody] EmployeeDto employeeDto)
		{
			if (employeeDto.Id == 0) //new
			{
                await Console.Out.WriteLineAsync("New user");
                await _context.Employees.AddAsync(new Employee
				{
					//username?
					EmailAddress = employeeDto.EmailAddress,
					Password = employeeDto.Password,
					Name = employeeDto.Name,
					BirthDate = employeeDto.BirthDate,
					Tasks = new List<TaskItem>(),
					PermissionLevel = employeeDto.ToModel().PermissionLevel,
				});
            }
			else //edit
			{
				var employeeInDb = await _context.Employees
					.Include(e => e.Tasks)
					.FirstOrDefaultAsync(e => e.Id == employeeDto.Id);

                await Console.Out.WriteLineAsync($"(cntrl) UserID={employeeDto.Id}");

                if (employeeInDb == null)
				{
					throw new KeyNotFoundException();
				}
				employeeInDb.Password = employeeDto.Password;
				employeeInDb.Name = employeeDto.Name;
				employeeInDb.BirthDate = employeeDto.BirthDate;
				employeeInDb.PermissionLevel = employeeDto.ToModel().PermissionLevel;

				List<TaskItem> tasks = new();
				foreach(var t in employeeDto.Tasks)
				{
					tasks.Add(t.ToModel());
				}
				employeeInDb.Tasks = tasks;
			}
			await _context.SaveChangesAsync();
			return employeeDto;
		}

		//Get
		[HttpGet("GetEmployee/{id:int}")]
		public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
		{
			var res = await _context.Employees
				.Include(e => e.Tasks)
				.FirstOrDefaultAsync(e => e.Id == id);
			if (res == null)
			{
				throw new KeyNotFoundException();
			}
			return res.ToDto();
		}

		[HttpGet("GetEmployeeByEmail/{email}")]
		public async Task<ActionResult<EmployeeDto>> GetEmployee(string email)
		{
			var res = await _context.Employees
				.Include(e => e.Tasks)
				.FirstOrDefaultAsync(e => e.EmailAddress == email);
			if (res == null)
			{
				throw new KeyNotFoundException();
			}
			return res.ToDto();
		}

        //Get all
        [HttpGet("GetAllEmployees")]
		public async Task<ActionResult<List<EmployeeDto>>> GetEmployees()
		{
			try
			{
				var results = await _context.Employees
					.Include(e => e.Tasks)
					.ToListAsync();
        return results.Select(e => e.ToDto()).ToList();
			}
			catch (Exception e)
			{
				//Console.ForegroundColor = ConsoleColor.Yellow;
				//Console.WriteLine(e.StackTrace);

				//Console.ForegroundColor = ConsoleColor.Red;
				//await Console.Out.WriteLineAsync("###############Hiba van");
				//Console.ResetColor();
			}
			//Console.ForegroundColor = ConsoleColor.Red;
			//await Console.Out.WriteLineAsync("###############Ez nem fut le");
			//Console.ResetColor();
			return BadRequest();
		}

		//Get the employee's tasks
		[HttpGet("GetTasks/{employeeId:int}")]
		public async Task<ActionResult<List<TaskItemDto>>> GetTasks(int employeeId)
		{
			var employee = await _context.Employees
				.Include(e => e.Tasks)
				.FirstOrDefaultAsync(e => e.Id == employeeId);
			if(employee == null)
			{
				throw new KeyNotFoundException();
			}
			return employee.Tasks.Select(t => t.ToDto()).ToList()!;
		}

		//Delete
		[HttpDelete("DeleteEmployee/{id:int}")]
		public async Task<ActionResult> DeleteEmployee(int id)
		{
			var res = await _context.Employees
				.Include(e => e.Tasks)
				.FirstOrDefaultAsync(e => e.Id == id);
			if (res == null)
			{
				throw new KeyNotFoundException();
			}
			_context.Employees.Remove(res);
			await _context.SaveChangesAsync();
			return Ok();
		}
	}
}
