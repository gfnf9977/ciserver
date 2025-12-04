using System.Net.Http.Json;
using CiServer.Core.Entities;
using CiServer.Core.Commands;

const string SERVER_URL = "http://localhost:5086"; 
using var httpClient = new HttpClient { BaseAddress = new Uri(SERVER_URL) };

Console.WriteLine($"--- CI AGENT STARTED ---");
Console.WriteLine($"Connecting to server: {SERVER_URL}");

while (true)
{
    try
    {
        var response = await httpClient.GetAsync("/api/agent/work");

        if (response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.NoContent)
        {
            var build = await response.Content.ReadFromJsonAsync<Build>();
            if (build != null)
            {
                Console.WriteLine($"\n[AGENT] Received Job: Build {build.BuildId} for repo {build.Project?.RepoUrl}");
                
                await ProcessBuild(build, httpClient);
            }
        }
        else
        {
            Console.Write("."); 
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n[AGENT] Error connecting to server: {ex.Message}");
    }

    await Task.Delay(3000);
}

static async Task ProcessBuild(Build build, HttpClient client)
{
    var pipeline = new BuildPipeline();

    var cmd1 = new CloneRepositoryCommand(build);
    var cmd2 = new CompileCodeCommand(build);
    var cmd3 = new RunTestsCommand(build);

    pipeline.AddCommand(new HttpReportDecorator(cmd1, client, build.BuildId));
    pipeline.AddCommand(new HttpReportDecorator(cmd2, client, build.BuildId));
    pipeline.AddCommand(new HttpReportDecorator(cmd3, client, build.BuildId));

    try 
    {
        pipeline.Run();
        build.Status = BuildStatus.Success;
    }
    catch
    {
        build.Status = BuildStatus.Failed;
    }

    await client.PostAsJsonAsync("/api/agent/finish", build);
    Console.WriteLine("[AGENT] Job Finished. Waiting for new work...");
}

public class HttpReportDecorator : CommandDecorator
{
    private readonly HttpClient _client;
    private readonly Guid _buildId;

    public HttpReportDecorator(ICommand command, HttpClient client, Guid buildId) : base(command)
    {
        _client = client;
        _buildId = buildId;
    }

    public override void Execute()
    {
        string cmdName = _wrappedCommand.GetType().Name;
        SendLog($"[Agent] Started {cmdName}...");
        
        base.Execute(); 

        SendLog($"[Agent] Finished {cmdName}.");
    }

    private void SendLog(string content)
    {
        _client.PostAsJsonAsync("/api/agent/log", new BuildLog 
        { 
            BuildId = _buildId, 
            Content = content 
        });
    }
}