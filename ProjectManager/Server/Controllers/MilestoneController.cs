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
public class MilestoneController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public MilestoneController(ApplicationDbContext context)
    {
        _context = context;
    }

	//Create/Edit
	[HttpPost("SaveMilestone")]
	public async Task<ActionResult> SaveMilestone([FromBody] MilestoneDto milestoneDto)
	{
		if (!ModelState.IsValid) await Console.Out.WriteLineAsync("Rossz formátum");
		else await Console.Out.WriteLineAsync("Jó formátum");

		if (milestoneDto.Id == 0) //TODO
		{
            await Console.Out.WriteLineAsync("Creating new milestone");
            await _context.Milestones.AddAsync(new Milestone
			{
				//Id = autoincrement...
				Name = milestoneDto.Name,
				Deadline = milestoneDto.Deadline,
				Description = milestoneDto.Description
			});
		}
		else
		{
			await Console.Out.WriteLineAsync("Updating milestone");
			var milestoneInDb = await _context.Milestones.FindAsync(milestoneDto.Id);
			if (milestoneInDb == null)
			{
                await Console.Out.WriteLineAsync("Couldn't find milestone");
                throw new KeyNotFoundException();
			}
			milestoneInDb.Name = milestoneDto.Name;
			milestoneInDb.Deadline = milestoneDto.Deadline;
			milestoneInDb.Description = milestoneDto.Description;
		}
		await _context.SaveChangesAsync();
        return Ok(milestoneDto);
	}

	//Get
	[HttpGet("Get/{id}")]
	public async Task<ActionResult> GetMilestone(int id)
	{
		var result = await _context.Milestones.FindAsync(id);
		if (result == null)
		{
            await Console.Out.WriteLineAsync("Couldn't GET milestone");
            throw new KeyNotFoundException();
		}

		return Ok(result.ToDto());
	}

    [HttpGet("GetLast")]
    public async Task<ActionResult> GetLast()
    {
        var result = await _context.Milestones.FindAsync(_context.Milestones.Max(m => m.Id));
        if (result == null)
        {
            await Console.Out.WriteLineAsync("Couldn't GET milestone");
            throw new KeyNotFoundException();
        }

        return Ok(result.ToDto());
    }

    //Get all
    [HttpGet("GetAllMilestones")]
	public async Task<ActionResult<List<MilestoneDto>>> GetMilestones()
	{
		var result = await _context.Milestones.ToListAsync();
		return Ok(result.Select(e => e.ToDto()).ToList());
	}

	//Delete
	[HttpDelete("Delete/{id}")]
	public async Task<ActionResult> DeleteMilestone(int id)
	{
		var result = await _context.Milestones.FindAsync(id);
		if (result == null)
		{
            await Console.Out.WriteLineAsync("Did not found deletable");
            throw new KeyNotFoundException();
		}
		_context.Milestones.Remove(result);
		await _context.SaveChangesAsync();
		return Ok();
	}


	//test
	[HttpDelete("DeleteAll")]
	public async Task<ActionResult> DeleteAll()
	{
		foreach(var x in _context.Milestones)
		{
			_context.Milestones.Remove(x);
		}
		await _context.SaveChangesAsync();
		return Ok();
	}

}