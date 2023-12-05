using Microsoft.Data.SqlClient;
using Task = Workshop.Example.Api.Models.Task;

namespace Workshop.Example.Api.Helpers
{
    public class DatabaseHelper
    {
        private readonly IConfiguration _configuration;

        public DatabaseHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> CreateTaskAsync(Task task)
        {
            // Get the connection string from the configuration
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            connection.Open();

            string query = "INSERT INTO Tasks (Title, Description, IsCompleted) OUTPUT INSERTED.ID VALUES (@Title, @Description, @IsCompleted)";

            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);

            object? result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public IEnumerable<Task> GetAllTasks()
        {
            // Implement DB interaction to get all tasks
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            connection.Open();

            string query = "SELECT * FROM Tasks";

            using SqlCommand command = new(query, connection);
            using SqlDataReader reader = command.ExecuteReader();
            List<Task> tasks = new();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Task task = new()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        IsCompleted = reader.GetBoolean(reader.GetOrdinal("IsCompleted"))
                    };
                    tasks.Add(task);
                }
            }
            reader.Close();
            return tasks;
        }

        public void DeleteTask(int id)
        {
            // Implement DB interaction to delete a task by id
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            connection.Open();

            string query = "DELETE FROM Tasks WHERE Id = @Id";

            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            command.ExecuteNonQuery();

        }
    }
}
