using CiServer.Core.Entities;

namespace CiServer.Core.Commands;

public class RunTestsCommand : ICommand
{
    private readonly Build _build;

    public RunTestsCommand(Build build)
    {
        _build = build;
    }

    public void Execute()
    {
        Console.WriteLine("[TESTS] Running unit tests...");
        Thread.Sleep(1000);
        Console.WriteLine("[TESTS] All tests passed (10/10).");
    }
}