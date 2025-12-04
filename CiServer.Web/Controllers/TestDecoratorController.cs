using CiServer.Core.Entities;
using CiServer.Core.Commands;
using Microsoft.AspNetCore.Mvc;

namespace CiServer.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestDecoratorController : ControllerBase
{
    [HttpGet]
    public IActionResult RunDecoratedPipeline()
    {
        var build = new Build { BuildId = Guid.NewGuid() };
        Console.WriteLine($"\n--- TESTING DECORATOR PATTERN ---");

        var pipeline = new BuildPipeline();

        var gitCmd = new CloneRepositoryCommand(build);
        var compileCmd = new CompileCodeCommand(build);
        
        var decoratedGit = new LoggingDecorator(gitCmd);
        var decoratedCompile = new LoggingDecorator(compileCmd);

        pipeline.AddCommand(decoratedGit);
        pipeline.AddCommand(decoratedCompile);
        
        pipeline.AddCommand(new RunTestsCommand(build));

        pipeline.Run();

        return Ok("Decorated pipeline executed. Check console.");
    }
}