using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.DTOs;

public class CreateTaskDto
{
    [Required]
    public required string Title { get; set; }
    
    public DateTime? DueDate { get; set; }
}

public class UpdateTaskDto
{
    public string? Title { get; set; }
    public DateTime? DueDate { get; set; }
    public bool? IsCompleted { get; set; }
}

public class TaskDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public int ProjectId { get; set; }
}
