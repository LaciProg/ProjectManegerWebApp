using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectManager.Server.Data.Models;


[Serializable]
[Index(nameof(EmailAddress), IsUnique = true)]
public record Employee {
    public enum PermissionType {
        Admin,
        ProjectManager,
        Employee
    }

    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public List<TaskItem> Tasks { get; set; }
    public PermissionType PermissionLevel { get; set; } = PermissionType.Employee;
}