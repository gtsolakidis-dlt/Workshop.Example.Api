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
            // Implement DB interaction to create new task
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

        public void UpdateTask(Task task)
        {
            // Implement DB interaction to update a task
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            connection.Open();

            string query = "UPDATE Tasks SET"; // Title = @Title, Description = @Description, IsCompleted = @IsCompleted WHERE Id = @Id";
            using SqlCommand command = new(query, connection);
            var updateFields = new List<string>();
            foreach (var property in task.GetType().GetProperties())
            {
                if (property.Name != "Id")
                {
                    updateFields.Add($"{property.Name} = @{property.Name}");
                    command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(task));

                }
            }

            query += " " + string.Join(", ", updateFields);
            query += " WHERE Id = @Id";
            command.CommandText = query;

            command.ExecuteNonQuery();
        }

        public Task GetTask(Task task)
        {
            // Implement DB interaction to get a task by a combination of not null properties of the recieved task
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            using SqlConnection connection = new(connectionString);
            connection.Open();

            string query = "SELECT * FROM Tasks WHERE";
            var whereClauses = new List<string>();
            foreach (var property in task.GetType().GetProperties())
            {
                if (property.GetValue(task) != null)
                {
                    whereClauses.Add($"{property.Name} = @{property.Name}");
                }
            }

            query += " " + string.Join(" AND ", whereClauses);

            using SqlCommand command = new(query, connection);
            foreach (var property in task.GetType().GetProperties())
            {
                if (property.GetValue(task) != null)
                {
                    command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(task));
                }
            }

            using SqlDataReader reader = command.ExecuteReader();
            Task result = new();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    result.Title = reader.GetString(reader.GetOrdinal("Title"));
                    result.Description = reader.GetString(reader.GetOrdinal("Description"));
                    result.IsCompleted = reader.GetBoolean(reader.GetOrdinal("IsCompleted"));
                }
            }
            reader.Close();
            return result;
        }
    }
}
