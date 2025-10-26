using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.DTOs;

public class CreateProjectDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string Title { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
}

public class ProjectDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
