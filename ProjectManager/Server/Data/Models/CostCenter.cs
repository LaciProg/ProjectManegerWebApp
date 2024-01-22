using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Server.Data.Models;

[Serializable]
public class CostCenter
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public Employee Manager { get; set; }
    public DateTime Deadline { get; set; }
    public Project Project { get; set; }
}