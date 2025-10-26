using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.DTOs;

public class ScheduleRequestDto
{
    [Required]
    public required List<ScheduleTaskDto> Tasks { get; set; }
}

public class ScheduleTaskDto
{
    [Required]
    public required string Title { get; set; }
    
    [Required]
    [Range(0.1, double.MaxValue, ErrorMessage = "Estimated hours must be greater than 0")]
    public double EstimatedHours { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    public List<string> Dependencies { get; set; } = new();
}

public class ScheduleResponseDto
{
    public required List<string> RecommendedOrder { get; set; }
}
