using System.ComponentModel.DataAnnotations;

namespace CiServer.Core.Entities;

public class Project
{
    [Key]
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RepoUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Build> Builds { get; set; } = new List<Build>();
}