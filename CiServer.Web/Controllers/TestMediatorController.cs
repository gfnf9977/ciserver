using CiServer.Core.Entities;
using CiServer.Core.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace CiServer.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestMediatorController : ControllerBase
{
    [HttpGet]
    public IActionResult TestMediator()
    {
        Console.WriteLine("\n--- TESTING MEDIATOR PATTERN ---");

        var buildComp = new BuildProcessComponent();
        var notifComp = new NotificationComponent();
        var dbComp = new DbArchiverComponent();

        var mediator = new CiMediator(buildComp, notifComp, dbComp);

        var build = new Build { BuildId = Guid.NewGuid() };

        buildComp.RunBuildSimulation(build);

        return Ok("Mediator logic executed. Check console.");
    }
}