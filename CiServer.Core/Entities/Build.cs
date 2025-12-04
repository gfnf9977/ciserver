using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CiServer.Core.Entities;
public class Build
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
}