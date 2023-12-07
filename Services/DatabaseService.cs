﻿using Microsoft.Data.SqlClient;
using Workshop.Example.Api.Interfaces;
using Workshop.Example.Api.Models.Requests;
using Workshop.Example.Api.Models.Responses;
using Task = Workshop.Example.Api.Models.Common.Task;

namespace Workshop.Example.Api.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DatabaseService> _logger;

        private const string CREATIONSTATUS = "Created";

        public DatabaseService(IConfiguration configuration, ILogger<DatabaseService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<CreateTaskResponse> CreateTaskAsync(CreateTaskRequest createTaskRequest)
        {
            try
            {
                // Implement DB interaction to create new task
                _logger.LogInformation("Creating task...");

                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection connection = new(connectionString);
                connection.Open();

                string query = "INSERT INTO Tasks (" +
                                "   Title, " +
                                "   Description, " +
                                "   Assignee, " +
                                "   AssignmentDate, " +
                                "   StartDate, " +
                                "   DueDate, " +
                                "   CompletionDate, " +
                                "   Status, " +
                                "   CompletionPercentage, " +
                                "   Notes" +
                                ") " +
                                "OUTPUT INSERTED.ID " +
                                "VALUES (" +
                                "   @Title, " +
                                "   @Description, " +
                                "   @Assignee, " +
                                "   @AssignmentDate, " +
                                "   @StartDate, " +
                                "   @DueDate, " +
                                "   @CompletionDate, " +
                                "   @Status, " +
                                "   @CompletionPercentage, " +
                                "   @Notes" +
                                ")";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Title", createTaskRequest.Title);
                command.Parameters.AddWithValue("@Description", createTaskRequest.Description);
                command.Parameters.AddWithValue("@Assignee", createTaskRequest.Assignee);
                command.Parameters.AddWithValue("@AssignmentDate", createTaskRequest.AssignmentDate);
                command.Parameters.AddWithValue("@StartDate", createTaskRequest.StartDate);
                command.Parameters.AddWithValue("@DueDate", createTaskRequest.DueDate);
                command.Parameters.AddWithValue("@CompletionDate", createTaskRequest.CompletionDate);
                command.Parameters.AddWithValue("@Status", CREATIONSTATUS);
                command.Parameters.AddWithValue("@CompletionPercentage", createTaskRequest.CompletionPercentage);
                command.Parameters.AddWithValue("@Notes", createTaskRequest.Notes);

                object? result = await command.ExecuteScalarAsync();

                _logger.LogInformation("Task created successfully.");

                return new CreateTaskResponse
                {
                    Id = (int)result,
                    Title = createTaskRequest.Title,
                    Status = CREATIONSTATUS
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating task.");
                throw;
            }
        }

        public async Task<IEnumerable<Task>> GetAllTasksAsync()
        {
            try
            {
                // Implement DB interaction to get all tasks
                _logger.LogInformation("Getting all tasks...");

                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection connection = new(connectionString);
                await connection.OpenAsync();

                string query = "SELECT * FROM Tasks";

                using SqlCommand command = new(query, connection);
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                List<Task> tasks = new();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        Task task = new()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString(reader.GetOrdinal("Description")),
                            Assignee = reader.IsDBNull(reader.GetOrdinal("Assignee")) ? "" : reader.GetString(reader.GetOrdinal("Assignee")),
                            AssignmentDate = reader.GetDateTime(reader.GetOrdinal("AssignmentDate")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                            CompletionDate = reader.GetDateTime(reader.GetOrdinal("CompletionDate")),
                            Status = reader.GetString(reader.GetOrdinal("Status")),
                            CompletionPercentage = reader.GetInt32(reader.GetOrdinal("CompletionPercentage")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? "" : reader.GetString(reader.GetOrdinal("Notes"))
                        };
                        tasks.Add(task);
                    }
                }
                reader.Close();

                _logger.LogInformation("Tasks fetched successfully.");

                return tasks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating task.");
                throw;
            }
        }

        public async Task<DeleteTaskResponse> DeleteTaskAsync(DeleteTaskRequest deleteTaskRequest)
        {
            try
            {
                // Implement DB interaction to delete a task by id
                _logger.LogInformation("Deleting task...");

                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection connection = new(connectionString);
                connection.Open();

                string query = "DELETE FROM Tasks WHERE Id = @Id";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Id", deleteTaskRequest.Id);

                await command.ExecuteNonQueryAsync();

                _logger.LogInformation("Task deleted successfully.");

                return new DeleteTaskResponse
                {
                    Result = true,
                    ResultMessage = $"Task [ID:{deleteTaskRequest.Id}] deleted successfully!",
                    Timestamp = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting task.");
                throw;
            }
        }

        public async Task<UpdateTaskResponse> UpdateTaskAsync(UpdateTaskRequest updateTaskRequest)
        {
            try
            {
                // Implement DB interaction to update a task
                _logger.LogInformation("Updating task...");

                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection connection = new(connectionString);
                connection.Open();

                string query = "UPDATE Tasks SET";
                using SqlCommand command = new(query, connection);
                var updateFields = new List<string>();
                foreach (var property in updateTaskRequest.GetType().GetProperties())
                {
                    var value = property.GetValue(updateTaskRequest);
                    if (property.Name != "Id" && value != null)
                    {
                        updateFields.Add($"{property.Name} = @{property.Name}");
                        command.Parameters.AddWithValue($"@{property.Name}", value);
                    }
                }

                query += " " + string.Join(", ", updateFields);
                query += " WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", updateTaskRequest.Id);
                command.CommandText = query;

                await command.ExecuteNonQueryAsync();

                _logger.LogInformation("Task updated successfully.");

                return new UpdateTaskResponse
                {
                    Result = true,
                    ResultMessage = $"Task [ID: {updateTaskRequest.Id}] updated successfully!",
                    Timestamp = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating task.");
                throw;
            }
        }

        public async Task<GetTaskResponse> GetTaskAsync(GetTaskRequest getTaskRequest)
        {
            try
            {
                // Implement DB interaction to get a task by a combination of not null properties of the recieved task
                _logger.LogInformation("Searching for task...");

                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection connection = new(connectionString);
                await connection.OpenAsync();

                string query = "SELECT * FROM Tasks WHERE";
                var whereClauses = new List<string>();
                foreach (var property in getTaskRequest.GetType().GetProperties())
                {
                    if (property.GetValue(getTaskRequest) != null)
                    {
                        whereClauses.Add($"{property.Name} = @{property.Name}");
                    }
                }

                query += " " + string.Join(" AND ", whereClauses);

                using SqlCommand command = new(query, connection);
                foreach (var property in getTaskRequest.GetType().GetProperties())
                {
                    if (property.GetValue(getTaskRequest) != null)
                    {
                        command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(getTaskRequest));
                    }
                }

                using SqlDataReader reader = await command.ExecuteReaderAsync();
                GetTaskResponse result = new();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        result.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        result.Title = reader.GetString(reader.GetOrdinal("Title"));
                        result.Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString(reader.GetOrdinal("Description"));
                        result.Assignee = reader.IsDBNull(reader.GetOrdinal("Assignee")) ? "" : reader.GetString(reader.GetOrdinal("Assignee"));
                        result.AssignmentDate = reader.GetDateTime(reader.GetOrdinal("AssignmentDate"));
                        result.StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate"));
                        result.DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate"));
                        result.CompletionDate = reader.GetDateTime(reader.GetOrdinal("CompletionDate"));
                        result.Status = reader.GetString(reader.GetOrdinal("Status"));
                        result.CompletionPercentage = reader.GetInt32(reader.GetOrdinal("CompletionPercentage"));
                        result.Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? "" : reader.GetString(reader.GetOrdinal("Notes"));
                    }
                }
                reader.Close();

                _logger.LogInformation("Task searched successfully.");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for task.");
                throw;
            }
        }
    }
}
