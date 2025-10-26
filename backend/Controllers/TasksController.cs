using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementAPI.Data;
using ProjectManagementAPI.DTOs;

namespace ProjectManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public TasksController(AppDbContext context)
    {
        _context = context;
    }
    
    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto dto)
    {
        var userId = GetUserId();
        var task = await _context.Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id && t.Project!.UserId == userId);
            
        if (task == null)
            return NotFound();
            
        if (dto.Title != null)
            task.Title = dto.Title;
        if (dto.DueDate.HasValue)
            task.DueDate = dto.DueDate;
        if (dto.IsCompleted.HasValue)
            task.IsCompleted = dto.IsCompleted.Value;
            
        await _context.SaveChangesAsync();
        
        var taskDto = new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            ProjectId = task.ProjectId
        };
        
        return Ok(taskDto);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var userId = GetUserId();
        var task = await _context.Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id && t.Project!.UserId == userId);
            
        if (task == null)
            return NotFound();
            
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}
