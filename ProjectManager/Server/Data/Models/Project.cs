using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Server.Data.Models;

[Serializable]
public record Project {
    public enum StatusType {
        Active,
        Archived,
        Complete
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }
    public string Goal { get; set; }
    public string Description { get; set; }
    public StatusType Status { get; set; } = StatusType.Active;
    public List<TaskItem> Tasks { get; set; } = new();
    public List<Employee> Assignees { get; set; } = new();
    public Employee? ProjectManager { get; set; }
    public List<Milestone> Milestones { get; set; } = new();
    public DateTime Created { get; set; } = DateTime.Now;
}