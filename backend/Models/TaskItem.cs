using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.Models;

public class TaskItem
{
    public int Id { get; set; }
    
    [Required]
    public required string Title { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
}
