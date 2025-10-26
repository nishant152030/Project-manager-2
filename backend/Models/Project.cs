using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.Models;

public class Project
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string Title { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
