using ProjectManagementAPI.DTOs;

namespace ProjectManagementAPI.Services;

public class SchedulerService : ISchedulerService
{
    public ScheduleResponseDto CalculateSchedule(ScheduleRequestDto request)
    {
        var tasks = request.Tasks;
        
        // Validate that all dependencies exist
        var taskTitles = new HashSet<string>(tasks.Select(t => t.Title));
        foreach (var task in tasks)
        {
            foreach (var dep in task.Dependencies)
            {
                if (!taskTitles.Contains(dep))
                {
                    throw new ArgumentException($"Task '{task.Title}' has unknown dependency '{dep}'");
                }
            }
        }
        
        // Build adjacency list and in-degree map
        var inDegree = new Dictionary<string, int>();
        var adjacencyList = new Dictionary<string, List<string>>();
        var taskMap = tasks.ToDictionary(t => t.Title);
        
        foreach (var task in tasks)
        {
            inDegree[task.Title] = 0;
            adjacencyList[task.Title] = new List<string>();
        }
        
        foreach (var task in tasks)
        {
            foreach (var dep in task.Dependencies)
            {
                adjacencyList[dep].Add(task.Title);
                inDegree[task.Title]++;
            }
        }
        
        // Topological sort using Kahn's algorithm with priority based on due date
        var result = new List<string>();
        var queue = new PriorityQueue<string, (DateTime dueDate, double estimatedHours)>();
        
        // Add all tasks with no dependencies
        foreach (var task in tasks)
        {
            if (inDegree[task.Title] == 0)
            {
                queue.Enqueue(task.Title, (task.DueDate, task.EstimatedHours));
            }
        }
        
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current);
            
            // Process all tasks that depend on the current task
            foreach (var dependent in adjacencyList[current])
            {
                inDegree[dependent]--;
                
                if (inDegree[dependent] == 0)
                {
                    var dependentTask = taskMap[dependent];
                    queue.Enqueue(dependent, (dependentTask.DueDate, dependentTask.EstimatedHours));
                }
            }
        }
        
        // Check for cycles
        if (result.Count != tasks.Count)
        {
            throw new InvalidOperationException("Circular dependency detected in tasks");
        }
        
        return new ScheduleResponseDto
        {
            RecommendedOrder = result
        };
    }
}
