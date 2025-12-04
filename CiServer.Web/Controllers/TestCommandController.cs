using CiServer.Core.Entities;
using CiServer.Core.Commands;
using Microsoft.AspNetCore.Mvc;

namespace CiServer.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestCommandController : ControllerBase
{
    [HttpGet]
    public IActionResult RunPipeline()
    {
        // 1. Підготовка даних (Mock)
        var project = new Project { Name = "SuperApp", RepoUrl = "https://github.com/user/superapp.git" };
        var build = new Build { BuildId = Guid.NewGuid(), Project = project };

        Console.WriteLine($"\n--- TESTING COMMAND PATTERN for Build {build.BuildId} ---");

        // 2. Створення Інвокера (Пайплайну)
        var pipeline = new BuildPipeline();

        // 3. Наповнення командами
        pipeline.AddCommand(new CloneRepositoryCommand(build));
        pipeline.AddCommand(new CompileCodeCommand(build));
        pipeline.AddCommand(new RunTestsCommand(build));

        // 4. Виконання (тут можна також викликати build.Start(), якщо поєднувати з State)
        build.Start(); // Перехід в Running (з минулої лаби)
        
        pipeline.Run();
        
        build.Finish(true); // Перехід в Success (з минулої лаби)

        return Ok("Pipeline executed. Check console output.");
    }
}