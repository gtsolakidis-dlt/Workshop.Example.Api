using Workshop.Example.Api.Models.Requests;
using Workshop.Example.Api.Models.Responses;
using Task = Workshop.Example.Api.Models.Common.Task;

namespace Workshop.Example.Api.Interfaces
{
    public interface IDatabaseService
    {
        public abstract Task<CreateTaskResponse> CreateTaskAsync(CreateTaskRequest createTaskRequest);
        public abstract Task<IEnumerable<Task>> GetAllTasksAsync();
        public abstract Task<DeleteTaskResponse> DeleteTaskAsync(DeleteTaskRequest deleteTaskRequest);
        public abstract Task<UpdateTaskResponse> UpdateTaskAsync(UpdateTaskRequest updateTaskRequest);
        public abstract Task<GetTaskResponse> GetTaskAsync(GetTaskRequest getTaskRequest);
    }
}
