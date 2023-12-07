using Workshop.Example.Api.Interfaces;
using Workshop.Example.Api.Models.Requests;
using Workshop.Example.Api.Models.Responses;
using Task = Workshop.Example.Api.Models.Common.Task;

namespace Workshop.Example.Api.Services
{
    public class TasksService : ITasksService
    {
        private readonly IDatabaseService _databaseService;

        public TasksService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<CreateTaskResponse> CreateTaskAsync(CreateTaskRequest request)
        {
            return await _databaseService.CreateTaskAsync(request);
        }

        public async Task<UpdateTaskResponse> UpdateTaskAsync(UpdateTaskRequest request)
        {
            return await _databaseService.UpdateTaskAsync(request);
        }

        public async Task<DeleteTaskResponse> DeleteTaskAsync(DeleteTaskRequest request)
        {
            return await _databaseService.DeleteTaskAsync(request);
        }

        public async Task<GetTaskResponse> SearchTasksAsync(GetTaskRequest request)
        {
            return await _databaseService.GetTaskAsync(request);
        }

        public async Task<IEnumerable<Task>> GetAllTasksAsync()
        {
            return await _databaseService.GetAllTasksAsync();
        }
    }
}
