using Microsoft.AspNetCore.Mvc;
using Task = Workshop.Example.Api.Models.Common.Task;
using Workshop.Example.Api.Helpers;
using Workshop.Example.Api.Models.Requests;
using Workshop.Example.Api.Models.Responses;

namespace Workshop.Example.Api.Controllers
{
    public class TasksController : ControllerBase
    {
        private readonly DatabaseHelper _databaseHelper;

        public TasksController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        [HttpPost]
        [Route("api/tasks/add")]
        public async Task<ActionResult<CreateTaskResponse>> CreateTask([FromBody] CreateTaskRequest createTaskRequest)
        {
            return Ok(await _databaseHelper.CreateTaskAsync(createTaskRequest));
        }

        [HttpGet]
        [Route("api/tasks/get")]
        public ActionResult<List<Task>> GetAllTasks()
        {
            return Ok( _databaseHelper.GetAllTasks());
        }

        [HttpPost]
        [Route("api/tasks/delete")]
        public ActionResult<DeleteTaskResponse> DeleteTask([FromBody] DeleteTaskRequest deleteTaskRequest)
        {
            return Ok(_databaseHelper.DeleteTask(deleteTaskRequest));
        }

        [HttpPost]
        [Route("api/tasks/update")]
        public ActionResult<UpdatetaskResponse> UpdateTask([FromBody] UpdateTaskRequest updateTaskRequest)
        {
            return Ok(_databaseHelper.UpdateTask(updateTaskRequest));
        }

        [HttpPost]
        [Route("api/tasks/search")]
        public ActionResult<GetTaskResponse> GetTask([FromBody] GetTaskRequest getTaskRequest)
        {
            return Ok(_databaseHelper.GetTask(getTaskRequest));
        }
    }
}
