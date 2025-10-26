using ProjectManagementAPI.DTOs;

namespace ProjectManagementAPI.Services;

public interface ISchedulerService
{
    ScheduleResponseDto CalculateSchedule(ScheduleRequestDto request);
}
