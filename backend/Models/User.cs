namespace ProjectManagementAPI.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
