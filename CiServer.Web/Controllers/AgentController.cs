using CiServer.Data;
using CiServer.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CiServer.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AgentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AgentController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("work")]
    public async Task<IActionResult> GetWork()
    {
        var build = await _context.Builds
            .Include(b => b.Project)
            .FirstOrDefaultAsync(b => b.Status == BuildStatus.Pending);

        if (build == null)
            return NoContent(); 

        build.Status = BuildStatus.Running;
        build.StartTime = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        Console.WriteLine($"[SERVER] Gave build {build.BuildId} to an Agent.");
        
        return Ok(build);
    }

    [HttpPost("finish")]
    public async Task<IActionResult> FinishWork([FromBody] Build result)
    {
        var build = await _context.Builds.FindAsync(result.BuildId);
        if (build == null) return NotFound();

        build.Status = result.Status;
        build.EndTime = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        Console.WriteLine($"[SERVER] Build {build.BuildId} finished with status: {build.Status}");
        
        return Ok();
    }

    [HttpPost("log")]
    public async Task<IActionResult> AddLog([FromBody] BuildLog log)
    {
        log.Timestamp = DateTime.UtcNow;
        _context.BuildLogs.Add(log);
        await _context.SaveChangesAsync();
        return Ok();
    }
}