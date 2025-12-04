using CiServer.Core.Entities;

namespace CiServer.Core.Mediator;

public class CiMediator : IMediator
{
    private readonly BuildProcessComponent _buildComponent;
    private readonly NotificationComponent _notificationComponent;
    private readonly DbArchiverComponent _dbArchiverComponent;

    public CiMediator(
        BuildProcessComponent buildComponent, 
        NotificationComponent notificationComponent, 
        DbArchiverComponent dbArchiverComponent)
    {
        _buildComponent = buildComponent;
        _buildComponent.SetMediator(this);

        _notificationComponent = notificationComponent;
        _notificationComponent.SetMediator(this);

        _dbArchiverComponent = dbArchiverComponent;
        _dbArchiverComponent.SetMediator(this);
    }

    public void Notify(object sender, string ev, object? data = null)
    {
        if (ev == "BuildFinished")
        {
            Console.WriteLine("\n[Mediator] Reacting to 'BuildFinished' event...");
            
            var build = data as Build;

            _dbArchiverComponent.SaveResultsToDb(build);

            if (build.Status == BuildStatus.Success)
            {
                _notificationComponent.SendEmail($"Build {build.BuildId} was SUCCESSFUL!");
            }
            else
            {
                _notificationComponent.SendEmail($"Build {build.BuildId} FAILED!");
            }
        }
    }
}