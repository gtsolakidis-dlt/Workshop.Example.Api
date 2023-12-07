using Workshop.Example.Api.Interfaces;
using Workshop.Example.Api.Models.Requests;
using Workshop.Example.Api.Models.Responses;
using Task = Workshop.Example.Api.Models.Common.Task;

namespace Workshop.Example.Api.Services
{
    public class TasksService : ITasksService
    {
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<TasksService> _logger;

        public TasksService(IDatabaseService databaseService, ILogger<TasksService> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        public async Task<CreateTaskResponse> CreateTaskAsync(CreateTaskRequest request)
        {
            try
            {
                _logger.LogInformation("Creating task...");

                var response = await _databaseService.CreateTaskAsync(request);

                _logger.LogInformation("Task created successfully.");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating task.");
                throw;
            }
        }

        public async Task<UpdateTaskResponse> UpdateTaskAsync(UpdateTaskRequest request)
        {
            try
            {
                _logger.LogInformation("Updating task...");

                var response = await _databaseService.UpdateTaskAsync(request);

                _logger.LogInformation("Task updated successfully.");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating task.");
                throw;
            }
        }

        public async Task<DeleteTaskResponse> DeleteTaskAsync(DeleteTaskRequest request)
        {
            try
            {
                _logger.LogInformation("Deleting task...");

                var response = await _databaseService.DeleteTaskAsync(request);

                _logger.LogInformation("Task deleted successfully.");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting task.");
                throw;
            }
        }

        public async Task<GetTaskResponse> SearchTasksAsync(GetTaskRequest request)
        {
            try
            {
                _logger.LogInformation("Searching for tasks...");

                var response = await _databaseService.GetTaskAsync(request);

                _logger.LogInformation("Task searched successfully.");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for task.");
                throw;
            }
        }

        public async Task<IEnumerable<Task>> GetAllTasksAsync()
        {
            try
            {
                _logger.LogInformation("Getting all tasks...");

                var response = await _databaseService.GetAllTasksAsync();

                _logger.LogInformation("All tasks retrieved successfully.");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all tasks.");
                throw;
            }
        }
    }
}
