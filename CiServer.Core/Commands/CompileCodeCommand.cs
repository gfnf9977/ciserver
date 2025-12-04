using CiServer.Core.Entities;

namespace CiServer.Core.Commands;

public class CompileCodeCommand : ICommand
{
    private readonly Build _build;

    public CompileCodeCommand(Build build)
    {
        _build = build;
    }

    public void Execute()
    {
        Console.WriteLine($"[COMPILER] Compiling build {_build.BuildId}...");
        Thread.Sleep(1500);
        Console.WriteLine("[COMPILER] Compilation finished. No errors.");
    }
}