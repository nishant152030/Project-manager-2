import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../services/api';
import type { Project, Task, CreateTaskDto, UpdateTaskDto, ScheduleRequestDto, ScheduleResponseDto } from '../types';

const ProjectDetails = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [project, setProject] = useState<Project | null>(null);
  const [tasks, setTasks] = useState<Task[]>([]);
  const [showForm, setShowForm] = useState(false);
  const [showScheduler, setShowScheduler] = useState(false);
  const [title, setTitle] = useState('');
  const [dueDate, setDueDate] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [schedulerLoading, setSchedulerLoading] = useState(false);
  const [scheduledOrder, setScheduledOrder] = useState<string[]>([]);
  const [successMessage, setSuccessMessage] = useState('');

  useEffect(() => {
    fetchProject();
    fetchTasks();
  }, [id]);

  const fetchProject = async () => {
    try {
      const response = await api.get(`/projects/${id}`);
      setProject(response.data);
    } catch (err) {
      console.error('Failed to fetch project', err);
    }
  };

  const fetchTasks = async () => {
    try {
      const response = await api.get(`/projects/${id}/tasks`);
      setTasks(response.data);
    } catch (err) {
      console.error('Failed to fetch tasks', err);
    }
  };

  const handleCreateTask = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const dto: CreateTaskDto = {
        title,
        dueDate: dueDate || undefined,
      };
      await api.post(`/projects/${id}/tasks`, dto);
      setTitle('');
      setDueDate('');
      setShowForm(false);
      setSuccessMessage('✅ Task created successfully!');
      setTimeout(() => setSuccessMessage(''), 3000);
      fetchTasks();
    } catch (err: any) {
      const errorMessage = err.response?.data?.message || err.message || 'Failed to create task';
      setError(`❌ ${errorMessage}`);
      setTimeout(() => setError(''), 5000);
    } finally {
      setLoading(false);
    }
  };

  const handleScheduleTasks = async (scheduleTasks: ScheduleRequestDto) => {
    setError('');
    setSchedulerLoading(true);

    try {
      const response = await api.post<ScheduleResponseDto>(`/projects/${id}/schedule`, scheduleTasks);
      setScheduledOrder(response.data.recommendedOrder);
      setSuccessMessage('✅ Schedule calculated successfully!');
      setTimeout(() => setSuccessMessage(''), 3000);
    } catch (err: any) {
      const errorMessage = err.response?.data?.message || err.message || 'Failed to calculate schedule';
      setError(`❌ ${errorMessage}`);
      setScheduledOrder([]);
      setTimeout(() => setError(''), 5000);
    } finally {
      setSchedulerLoading(false);
    }
  };

  const handleToggleComplete = async (task: Task) => {
    try {
      const dto: UpdateTaskDto = { isCompleted: !task.isCompleted };
      await api.put(`/tasks/${task.id}`, dto);
      setSuccessMessage('✅ Task updated!');
      setTimeout(() => setSuccessMessage(''), 2000);
      fetchTasks();
    } catch (err: any) {
      const errorMessage = err.response?.data?.message || err.message || 'Failed to update task';
      setError(`❌ ${errorMessage}`);
      setTimeout(() => setError(''), 3000);
    }
  };

  const handleDeleteTask = async (taskId: number) => {
    if (!confirm('Are you sure you want to delete this task?')) return;

    try {
      await api.delete(`/tasks/${taskId}`);
      setSuccessMessage('✅ Task deleted!');
      setTimeout(() => setSuccessMessage(''), 2000);
      fetchTasks();
    } catch (err: any) {
      const errorMessage = err.response?.data?.message || err.message || 'Failed to delete task';
      setError(`❌ ${errorMessage}`);
      setTimeout(() => setError(''), 3000);
    }
  };

  if (!project) return <div className="container"><div className="empty-state">Loading...</div></div>;

  return (
    <div className="container">
      <button onClick={() => navigate('/dashboard')} className="btn back-button">
        ← Back to Dashboard
      </button>

      <div className="project-details">
        <h2>{project.title}</h2>
        {project.description && <p>{project.description}</p>}
      </div>

      <hr className="divider" />

      {successMessage && (
        <div className="success-message">{successMessage}</div>
      )}
      {error && <div className="error-message">{error}</div>}

      <div className="tasks-section">
        <h3>Tasks</h3>
        <div className="action-bar">
          <button onClick={() => setShowForm(!showForm)} className="btn btn-new">
            {showForm ? 'Cancel' : '+ Add Task'}
          </button>
          <button 
            onClick={() => {
              setShowScheduler(!showScheduler);
              setScheduledOrder([]);
            }} 
            className="btn btn-primary"
            disabled={tasks.length === 0}
          >
            {showScheduler ? 'Hide Scheduler' : '📅 Smart Schedule'}
          </button>
        </div>

        {showForm && (
          <div className="form-card">
            <h3>Create New Task</h3>
            <form onSubmit={handleCreateTask}>
              <div className="form-group">
                <label>Title</label>
                <input
                  type="text"
                  value={title}
                  onChange={(e) => setTitle(e.target.value)}
                  required
                  placeholder="Enter task title"
                />
              </div>
              <div className="form-group">
                <label>Due Date (optional)</label>
                <input
                  type="date"
                  value={dueDate}
                  onChange={(e) => setDueDate(e.target.value)}
                />
              </div>
              <button type="submit" className="btn btn-primary" disabled={loading}>
                {loading ? 'Adding...' : 'Add Task'}
              </button>
            </form>
          </div>
        )}

        {showScheduler && <SchedulerForm tasks={tasks} onSchedule={handleScheduleTasks} loading={schedulerLoading} />}

        {scheduledOrder.length > 0 && (
          <div className="schedule-result">
            <h4>📋 Recommended Task Order</h4>
            <ol className="scheduled-list">
              {scheduledOrder.map((taskTitle, index) => (
                <li key={index}>{taskTitle}</li>
              ))}
            </ol>
          </div>
        )}

        {tasks.length === 0 ? (
          <div className="empty-state">
            <p>No tasks yet. Add your first task!</p>
          </div>
        ) : (
          <div className="task-list">
            {tasks.map((task) => (
              <div key={task.id} className="task-item">
                <div className="task-content">
                  <input
                    type="checkbox"
                    checked={task.isCompleted}
                    onChange={() => handleToggleComplete(task)}
                    className="task-checkbox"
                  />
                  <div>
                    <span className={`task-title ${task.isCompleted ? 'completed' : ''}`}>
                      {task.title}
                    </span>
                    {task.dueDate && (
                      <small className="task-due-date">
                        Due: {new Date(task.dueDate).toLocaleDateString()}
                      </small>
                    )}
                  </div>
                </div>
                <div className="task-actions">
                  <button
                    onClick={() => handleDeleteTask(task.id)}
                    className="btn btn-danger"
                  >
                    Delete
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

interface SchedulerFormProps {
  tasks: Task[];
  onSchedule: (request: ScheduleRequestDto) => void;
  loading: boolean;
}

const SchedulerForm = ({ tasks, onSchedule, loading }: SchedulerFormProps) => {
  const [scheduleTasks, setScheduleTasks] = useState<Map<string, { estimatedHours: number; dependencies: string[] }>>(
    new Map(tasks.map(t => [t.title, { estimatedHours: 1, dependencies: [] }]))
  );
  const [validationError, setValidationError] = useState('');

  const updateHours = (title: string, hours: number) => {
    setScheduleTasks(prev => {
      const newMap = new Map(prev);
      const task = newMap.get(title) || { estimatedHours: 1, dependencies: [] };
      newMap.set(title, { ...task, estimatedHours: hours });
      return newMap;
    });
  };

  const toggleDependency = (taskTitle: string, depTitle: string) => {
    setScheduleTasks(prev => {
      const newMap = new Map(prev);
      const task = newMap.get(taskTitle) || { estimatedHours: 1, dependencies: [] };
      const deps = task.dependencies.includes(depTitle)
        ? task.dependencies.filter(d => d !== depTitle)
        : [...task.dependencies, depTitle];
      newMap.set(taskTitle, { ...task, dependencies: deps });
      return newMap;
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setValidationError('');

    // Validate that tasks without due dates get a default
    const hasTasksWithoutDueDate = tasks.some(t => !t.dueDate);
    if (hasTasksWithoutDueDate) {
      setValidationError('⚠️ Some tasks don\'t have due dates. They will use today\'s date.');
    }

    const request: ScheduleRequestDto = {
      tasks: tasks.map(t => ({
        title: t.title,
        estimatedHours: scheduleTasks.get(t.title)?.estimatedHours || 1,
        dueDate: t.dueDate || new Date().toISOString(),
        dependencies: scheduleTasks.get(t.title)?.dependencies || []
      }))
    };
    onSchedule(request);
  };

  return (
    <div className="form-card scheduler-form">
      <h3>🎯 Configure Task Schedule</h3>
      <p className="help-text">Set estimated hours and dependencies for each task. Hold Ctrl/Cmd to select multiple dependencies.</p>
      {validationError && <div className="warning-message">{validationError}</div>}
      <form onSubmit={handleSubmit}>
        {tasks.map(task => (
          <div key={task.id} className="scheduler-task-item">
            <div className="task-header">
              <strong>{task.title}</strong>
              <small>{task.dueDate ? `Due: ${new Date(task.dueDate).toLocaleDateString()}` : '⚠️ No due date'}</small>
            </div>
            <div className="scheduler-inputs">
              <div className="form-group-inline">
                <label>Estimated Hours:</label>
                <input
                  type="number"
                  min="0.5"
                  step="0.5"
                  value={scheduleTasks.get(task.title)?.estimatedHours || 1}
                  onChange={(e) => updateHours(task.title, parseFloat(e.target.value))}
                  required
                />
              </div>
              <div className="form-group-inline dependencies-group">
                <label>Dependencies (tasks that must be done first):</label>
                {tasks.filter(t => t.title !== task.title).length > 0 ? (
                  <div className="dependency-checkboxes">
                    {tasks.filter(t => t.title !== task.title).map(t => (
                      <label key={t.id} className="checkbox-label">
                        <input
                          type="checkbox"
                          checked={scheduleTasks.get(task.title)?.dependencies.includes(t.title) || false}
                          onChange={() => toggleDependency(task.title, t.title)}
                        />
                        <span>{t.title}</span>
                      </label>
                    ))}
                  </div>
                ) : (
                  <small className="no-deps">No other tasks available</small>
                )}
              </div>
            </div>
          </div>
        ))}
        <button type="submit" className="btn btn-primary" disabled={loading}>
          {loading ? (
            <>
              <span className="spinner"></span> Calculating...
            </>
          ) : (
            '🚀 Calculate Schedule'
          )}
        </button>
      </form>
    </div>
  );
};

export default ProjectDetails;
