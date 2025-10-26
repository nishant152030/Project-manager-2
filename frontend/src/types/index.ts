export interface User {
  username: string;
}

export interface Project {
  id: number;
  title: string;
  description?: string;
  createdAt: string;
}

export interface Task {
  id: number;
  title: string;
  dueDate?: string;
  isCompleted: boolean;
  projectId: number;
}

export interface CreateProjectDto {
  title: string;
  description?: string;
}

export interface CreateTaskDto {
  title: string;
  dueDate?: string;
}

export interface UpdateTaskDto {
  title?: string;
  dueDate?: string;
  isCompleted?: boolean;
}

export interface ScheduleTaskDto {
  title: string;
  estimatedHours: number;
  dueDate: string;
  dependencies: string[];
}

export interface ScheduleRequestDto {
  tasks: ScheduleTaskDto[];
}

export interface ScheduleResponseDto {
  recommendedOrder: string[];
}

export interface ScheduleTaskDto {
  title: string;
  estimatedHours: number;
  dueDate: string;
  dependencies: string[];
}

export interface ScheduleRequestDto {
  tasks: ScheduleTaskDto[];
}

export interface ScheduleResponseDto {
  recommendedOrder: string[];
}
