using CiServer.Core.Entities;

namespace CiServer.Core.Commands;

public class CloneRepositoryCommand : ICommand
{
    private readonly Build _build;

    public CloneRepositoryCommand(Build build)
    {
        _build = build;
    }

    public void Execute()
    {
        Console.WriteLine($"[GIT] Cloning repository from {_build.Project?.RepoUrl ?? "Unknown URL"}...");
        Thread.Sleep(1000); 
        Console.WriteLine("[GIT] Repository cloned successfully.");
    }
}