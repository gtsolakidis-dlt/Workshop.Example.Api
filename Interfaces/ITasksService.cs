using Workshop.Example.Api.Models.Requests;
using Workshop.Example.Api.Models.Responses;
using Task = Workshop.Example.Api.Models.Common.Task;

namespace Workshop.Example.Api.Interfaces
{
    public interface ITasksService
    {
        public abstract Task<CreateTaskResponse> CreateTaskAsync(CreateTaskRequest request);
        public abstract Task<IEnumerable<Task>> GetAllTasksAsync();
        public abstract Task<UpdateTaskResponse> UpdateTaskAsync(UpdateTaskRequest request);
        public abstract Task<DeleteTaskResponse> DeleteTaskAsync(DeleteTaskRequest request);
        public abstract Task<GetTaskResponse> SearchTasksAsync(GetTaskRequest request);
    }
}
