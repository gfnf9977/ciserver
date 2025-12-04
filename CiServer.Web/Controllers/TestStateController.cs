using CiServer.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CiServer.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestStateController : ControllerBase
{
    [HttpGet]
    public IActionResult TestPattern()
    {
        var build = new Build();
        Console.WriteLine($"\n--- Створено новий білд. Поточний статус: {build.Status} ---");

        Console.WriteLine("Спроба завершити до початку:");
        build.Finish(true); 

        Console.WriteLine("\nЗапуск білда:");
        build.Start();
        Console.WriteLine($"Статус після старту: {build.Status}");

        Console.WriteLine("Спроба запустити знову:");
        build.Start();

        Console.WriteLine("\nЗавершення білда (успіх):");
        build.Finish(true);
        Console.WriteLine($"Статус після фінішу: {build.Status}");

        Console.WriteLine("Спроба скасувати готовий:");
        build.Cancel();

        return Ok("Дивись логи в консолі сервера!");
    }
}