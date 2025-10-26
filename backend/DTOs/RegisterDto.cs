using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.DTOs;

public class RegisterDto
{
    [Required]
    public required string Username { get; set; }
    
    [Required]
    [MinLength(6)]
    public required string Password { get; set; }
}
