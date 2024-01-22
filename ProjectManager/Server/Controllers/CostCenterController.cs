using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Server.Data;
using ProjectManager.Server.Data.Models;
using ProjectManager.Shared;

namespace ProjectManager.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class CostCenterController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		public CostCenterController(ApplicationDbContext context)
		{
			_context = context;
		}


		//Create/Edit
		[HttpPost("SaveCostCenter")]
		public async Task<ActionResult<CostCenterDto>> SaveCostCenter([FromBody] CostCenterDto costCenterDto /*CostCenterDto costCenterDto, int projectId*/)
		{
			if (costCenterDto.Id == 0) //new TODO
			{
				await _context.CostCenter.AddAsync(new CostCenter
				{
					Name = costCenterDto.Name,
					Manager = costCenterDto.ToModel().Manager,
					Deadline = costCenterDto.Deadline,
					Project = (await _context.Projects
						.Include(p => p.ProjectManager)
						.Include(p => p.Assignees)
						.Include(p => p.Milestones)
						.Include(p => p.Tasks)
						.FirstOrDefaultAsync(p => p.Id == costCenterDto.Project.Id))!
					//id generálás?
				});
			}
			else //edit
			{
				var costCenterInDb = await _context.CostCenter
						.Include(e => e.Manager)
						.Include(e => e.Project)
						.FirstOrDefaultAsync(c => c.Id == costCenterDto.Id);
				
				if(costCenterInDb == null) {
					throw new KeyNotFoundException();
				}
				costCenterInDb.Name = costCenterDto.Name;
				costCenterInDb.Manager = costCenterDto.ToModel().Manager;
				costCenterInDb.Deadline = costCenterDto.Deadline;
				costCenterInDb.Project = costCenterDto.ToModel().Project;
			}
			await _context.SaveChangesAsync();
			return costCenterDto;
		}

		//Get
		[HttpGet("GetCostCenter/{id:int}")]
		public async Task<ActionResult<CostCenterDto>> GetCostCenter(int id)
		{
			var res = await _context.CostCenter
				.Include(e => e.Manager)
				.Include(e => e.Project)
				.FirstOrDefaultAsync(e => e.Id == id);
			
			if(res == null)
			{
				throw new KeyNotFoundException();
			}
			return res.ToDto();
		}

		//Get all
		[HttpGet("GetAllCostCenters")]
		public async Task<ActionResult<List<CostCenterDto>>> GetCostCenters()
		{
				var results = await _context.CostCenter
					.Include(e => e.Manager)
					.Include(e => e.Project)
					.ToListAsync();
				return results.Select(e => e.ToDto()).ToList();
		}

		//Delete
		[HttpDelete("DeleteCostCenter/{id:int}")]
		public async Task<ActionResult> DeleteCostCenter(int id)
		{
			var res = await _context.CostCenter
				.Include(e => e.Manager)
				.Include(e => e.Project)
				.FirstOrDefaultAsync(c => c.Id == id);
			
			if(res == null)
			{
				throw new KeyNotFoundException();
			}
			_context.CostCenter.Remove(res);
			await _context.SaveChangesAsync();
			return Ok();
		}

	}
}
