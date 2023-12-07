using Microsoft.AspNetCore.Mvc;
using Task = Workshop.Example.Api.Models.Common.Task;
using Workshop.Example.Api.Models.Requests;
using Workshop.Example.Api.Models.Responses;
using Workshop.Example.Api.Interfaces;

namespace Workshop.Example.Api.Controllers
{
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITasksService tasksService, ILogger<TasksController> logger)
        {
            _tasksService = tasksService;
            _logger = logger;
        }

        [HttpPost]
        [Route("api/tasks/add")]
        public async Task<ActionResult<CreateTaskResponse>> CreateTask([FromBody] CreateTaskRequest createTaskRequest)
        {
            try
            {
                return Ok(await _tasksService.CreateTaskAsync(createTaskRequest));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the task.");
                return StatusCode(500, "An error occurred while creating the task.");
            }
        }

        [HttpGet]
        [Route("api/tasks/get")]
        public async Task<ActionResult<List<Task>>> GetAllTasks()
        {
            try
            {
                return Ok(await _tasksService.GetAllTasksAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching tasks.");
                return StatusCode(500, "An error occurred while fetching tasks.");
            }
        }

        [HttpPost]
        [Route("api/tasks/delete")]
        public async Task<ActionResult<DeleteTaskResponse>> DeleteTask([FromBody] DeleteTaskRequest deleteTaskRequest)
        {
            try
            {
                return Ok(await _tasksService.DeleteTaskAsync(deleteTaskRequest));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the task.");
                return StatusCode(500, "An error occurred while deleteing the task.");
            }
        }

        [HttpPost]
        [Route("api/tasks/update")]
        public async Task<ActionResult<UpdateTaskResponse>> UpdateTask([FromBody] UpdateTaskRequest updateTaskRequest)
        {
            try
            {
                return Ok(await _tasksService.UpdateTaskAsync(updateTaskRequest));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the task.");
                return StatusCode(500, "An error occurred while updating the task.");
            }
        }

        [HttpPost]
        [Route("api/tasks/search")]
        public async Task<ActionResult<GetTaskResponse>> GetTask([FromBody] GetTaskRequest getTaskRequest)
        {
            try
            {
                return Ok(await _tasksService.SearchTasksAsync(getTaskRequest));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for the task.");
                return StatusCode(500, "An error occurred while searching fot the task.");
            }
        }
    }
}
