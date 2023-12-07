using Microsoft.AspNetCore.Mvc;
using Task = Workshop.Example.Api.Models.Common.Task;
using Workshop.Example.Api.Helpers;
using Workshop.Example.Api.Models.Requests;
using Workshop.Example.Api.Models.Responses;
using Workshop.Example.Api.Interfaces;

namespace Workshop.Example.Api.Controllers
{
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;

        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        [HttpPost]
        [Route("api/tasks/add")]
        public async Task<ActionResult<CreateTaskResponse>> CreateTask([FromBody] CreateTaskRequest createTaskRequest)
        {
            return Ok(await _tasksService.CreateTaskAsync(createTaskRequest));
        }

        [HttpGet]
        [Route("api/tasks/get")]
        public async Task<ActionResult<List<Task>>> GetAllTasks()
        {
            return Ok(await _tasksService.GetAllTasksAsync());
        }

        [HttpPost]
        [Route("api/tasks/delete")]
        public async Task<ActionResult<DeleteTaskResponse>> DeleteTask([FromBody] DeleteTaskRequest deleteTaskRequest)
        {
            return Ok(await _tasksService.DeleteTaskAsync(deleteTaskRequest));
        }

        [HttpPost]
        [Route("api/tasks/update")]
        public async Task<ActionResult<UpdateTaskResponse>> UpdateTask([FromBody] UpdateTaskRequest updateTaskRequest)
        {
            return Ok(await _tasksService.UpdateTaskAsync(updateTaskRequest));
        }

        [HttpPost]
        [Route("api/tasks/search")]
        public async Task<ActionResult<GetTaskResponse>> GetTask([FromBody] GetTaskRequest getTaskRequest)
        {
            return Ok(await _tasksService.SearchTasksAsync(getTaskRequest));
        }
    }
}
