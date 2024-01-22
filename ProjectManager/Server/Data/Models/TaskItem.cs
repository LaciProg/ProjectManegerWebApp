using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Server.Data.Models;

[Serializable]
public record TaskItem {
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Description { get; set; }
    public uint Priority { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime? Completed { get; set; } = null;
    public string Name { get; set; }
    public TaskItem? Parent { get; set; } = null; //might not have a parent
    public Project.StatusType Status { get; set; } = Project.StatusType.Active;
    public List<Employee> Assignees { get; set; } = new();
}