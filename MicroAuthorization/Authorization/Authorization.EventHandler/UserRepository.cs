using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Authorization.EventHandler;
public interface IUserRepository
{
    public Task AddUserAsync(Guid id, string fullName, string email);
    public Task RemoveUserAsync(Guid id);
}

public class UserRepository : IUserRepository
{
    private string _connectionString { get; init; }

    public UserRepository(IOptions<AuthorizationSettings> settings)
    {
        _connectionString = settings.Value.ConnectionStrings.Database;
    }

    public async Task AddUserAsync(Guid id, string fullName, string email)
    {
        var sql = @"INSERT INTO [User] (Id, FullName, Email) VALUES (@id, @fullName, @email)";

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync(sql, new { id, fullName, email });
        }
    }

    public async Task RemoveUserAsync(Guid id)
    {
        var sql = $"DELETE FROM [User] WHERE Id = @id";

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync(sql, new { id });
        }
    }
}
