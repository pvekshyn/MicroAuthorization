using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Access;

public interface IAccessRepository
{
    public Task<bool> CheckAccessAsync(Guid userId, Guid PermissionId);
}

public class AccessRepository : IAccessRepository
{
    private string _connectionString { get; init; }

    public AccessRepository(IOptions<AccessSettings> settings)
    {
        _connectionString = settings.Value.ConnectionStrings.Database;
    }

    public async Task<bool> CheckAccessAsync(Guid userId, Guid permissionId)
    {
        var sql = $"SELECT 1 FROM [AccessEntry] WHERE [UserId] = @userId AND [PermissionId] = @permissionId";

        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.QueryFirstOrDefaultAsync<bool>(sql, new { userId, permissionId });
        }
    }
}
