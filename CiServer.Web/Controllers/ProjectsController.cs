using CiServer.Data;
using CiServer.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CiServer.Web.Controllers;

public class ProjectsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var projects = await _context.Projects.ToListAsync();
        return View(projects);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Project project)
    {
        if (ModelState.IsValid)
        {
            project.ProjectId = Guid.NewGuid();
            project.CreatedAt = DateTime.UtcNow;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        return View(project);
    }

    [HttpGet]
    public async Task<IActionResult> StartBuild(Guid id)
    {
        var build = new Build
        {
            BuildId = Guid.NewGuid(),
            ProjectId = id,
            Status = BuildStatus.Pending,
            StartTime = DateTime.UtcNow
        };

        _context.Builds.Add(build);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
