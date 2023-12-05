using Microsoft.AspNetCore.Mvc;
using Task = Workshop.Example.Api.Models.Task;
using Workshop.Example.Api.Helpers;

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
        public async Task<ActionResult<int>> CreateTask(Task task)
        {
            task.IsCompleted = task.IsCompleted.HasValue ? task.IsCompleted : false;
            return Ok(await _databaseHelper.CreateTaskAsync(task));
        }

        [HttpGet]
        [Route("api/tasks/get")]
        public ActionResult<List<Task>> GetAllTasks()
        {
            return Ok( _databaseHelper.GetAllTasks());
        }

        [HttpDelete]
        [Route("api/tasks/delete/{id}")]
        public ActionResult<Task> DeleteTask(int id)
        {
            _databaseHelper.DeleteTask(id);
            return NoContent();
        }
    }
}
