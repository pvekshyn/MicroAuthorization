using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Roles;

public interface IPermissionRepository
{
    public Task<Permission> GetAsync(Guid id);
    public Task AddAsync(Permission permission);
    public Task RemoveAsync(Guid id);
}

public class PermissionRepository : IPermissionRepository
{
    private string _connectionString { get; init; }

    public PermissionRepository(IOptions<RolesSettings> settings)
    {
        _connectionString = settings.Value.ConnectionStrings.Database;
    }

    public async Task<Permission> GetAsync(Guid id)
    {
        var sql = $"SELECT * FROM Permission WHERE Id = @id";

        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.QuerySingleOrDefaultAsync<Permission>(sql, new { id });
        }
    }

    public async Task AddAsync(Permission permission)
    {
        var sql = $"INSERT INTO Permission (Id, Name) VALUES (@id, @name)";

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync(sql, new { id = permission.Id, name = permission.Name });
        }
    }

    public async Task RemoveAsync(Guid id)
    {
        var sql = $"DELETE FROM Permission WHERE Id = @id";

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.ExecuteAsync(sql, new { id });
        }
    }
}
