using Microsoft.AspNetCore.Mvc;
using Task = Workshop.Example.Api.Models.Task;

namespace Workshop.Example.Api.Controllers
{
    public class TasksController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Task> CreateTask(Task task)
        {
            return Ok(new Task()
            { 
                Id = task.Id, 
                Title = task.Title, 
                Description = $"Task Description: {task.Description}.",
                IsCompleted = false
            });
        }

        [HttpGet]
        public ActionResult<IEnumerable<Task>> GetAllTasks()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(new Task()
                {
                    Id = i,
                    Title = $"Task {i}",
                    Description = $"Task Description: {i}.",
                    IsCompleted = false
                });
            }
            return Ok(tasks);
        }

        [HttpDelete("{id}")]
        public ActionResult<Task> DeleteTask()
        {
            return NoContent();
        }
    }
}
