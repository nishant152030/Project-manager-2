using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementAPI.Data;
using ProjectManagementAPI.DTOs;
using ProjectManagementAPI.Models;
using ProjectManagementAPI.Services;

namespace ProjectManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ISchedulerService _schedulerService;
    
    public ProjectsController(AppDbContext context, ISchedulerService schedulerService)
    {
        _context = context;
        _schedulerService = schedulerService;
    }
    
    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    
    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var userId = GetUserId();
        var projects = await _context.Projects
            .Where(p => p.UserId == userId)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
            
        return Ok(projects);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProject(int id)
    {
        var userId = GetUserId();
        var project = await _context.Projects
            .Where(p => p.Id == id && p.UserId == userId)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                CreatedAt = p.CreatedAt
            })
            .FirstOrDefaultAsync();
            
        if (project == null)
            return NotFound();
            
        return Ok(project);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProject(CreateProjectDto dto)
    {
        var userId = GetUserId();
        var project = new Project
        {
            Title = dto.Title,
            Description = dto.Description,
            UserId = userId
        };
        
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        
        var projectDto = new ProjectDto
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            CreatedAt = project.CreatedAt
        };
        
        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, projectDto);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var userId = GetUserId();
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
            
        if (project == null)
            return NotFound();
            
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
    
    [HttpGet("{projectId}/tasks")]
    public async Task<IActionResult> GetTasks(int projectId)
    {
        var userId = GetUserId();
        var projectExists = await _context.Projects
            .AnyAsync(p => p.Id == projectId && p.UserId == userId);
            
        if (!projectExists)
            return NotFound();
            
        var tasks = await _context.Tasks
            .Where(t => t.ProjectId == projectId)
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                DueDate = t.DueDate,
                IsCompleted = t.IsCompleted,
                ProjectId = t.ProjectId
            })
            .ToListAsync();
            
        return Ok(tasks);
    }
    
    [HttpPost("{projectId}/tasks")]
    public async Task<IActionResult> CreateTask(int projectId, CreateTaskDto dto)
    {
        var userId = GetUserId();
        var projectExists = await _context.Projects
            .AnyAsync(p => p.Id == projectId && p.UserId == userId);
            
        if (!projectExists)
            return NotFound();
            
        var task = new TaskItem
        {
            Title = dto.Title,
            DueDate = dto.DueDate,
            ProjectId = projectId
        };
        
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        
        var taskDto = new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            ProjectId = task.ProjectId
        };
        
        return CreatedAtAction(nameof(GetTasks), new { projectId }, taskDto);
    }
    
    [HttpPost("{projectId}/schedule")]
    public async Task<IActionResult> CalculateSchedule(int projectId, ScheduleRequestDto request)
    {
        var userId = GetUserId();
        var projectExists = await _context.Projects
            .AnyAsync(p => p.Id == projectId && p.UserId == userId);
            
        if (!projectExists)
            return NotFound(new { message = "Project not found" });
        
        try
        {
            var schedule = _schedulerService.CalculateSchedule(request);
            return Ok(schedule);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
