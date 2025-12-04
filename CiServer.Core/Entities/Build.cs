using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CiServer.Core.States;
using CiServer.Core.Visitor;

namespace CiServer.Core.Entities;

public class Build : IElement
{
    [Key]
    public Guid BuildId { get; set; }

    [ForeignKey("Project")]
    public Guid ProjectId { get; set; }

    public string CommitHash { get; set; } = string.Empty;
    public BuildStatus Status { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Project? Project { get; set; }
    public ICollection<BuildLog> Logs { get; set; } = new List<BuildLog>();
    public ICollection<Artifact> Artifacts { get; set; } = new List<Artifact>();

    [NotMapped]
    private BuildState _state;

    public Build()
    {
        TransitionTo(new PendingState());
    }

    public void TransitionTo(BuildState state)
    {
        _state = state;
        _state.SetContext(this);
        UpdateDbStatus();
    }

    public void Start() => _state.StartBuild();
    public void Finish(bool success) => _state.FinishBuild(success);
    public void Cancel() => _state.CancelBuild();

    private void UpdateDbStatus()
    {
        if (_state is PendingState) Status = BuildStatus.Pending;
        else if (_state is RunningState) Status = BuildStatus.Running;
        else if (_state is SuccessState) Status = BuildStatus.Success;
        else if (_state is FailedState) Status = BuildStatus.Failed;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.VisitBuild(this);
        foreach (var log in Logs)
        {
            log.Accept(visitor);
        }
    }
}
