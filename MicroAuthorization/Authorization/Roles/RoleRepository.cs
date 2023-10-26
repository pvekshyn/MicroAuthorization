using Dapper;
using DotNetCore.CAP;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Roles;

public interface IRoleRepository
{
    public Task<Role> GetAsync(Guid id);
    public Task AddAsync(Role role);
    public Task AddPermissionToRoleAsync(Guid id, Guid permissionId);
    public Task RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId);
    public Task RemoveAsync(Guid id);
}

public class RoleRepository : IRoleRepository
{
    private string _connectionString { get; init; }
    private readonly ICapPublisher _capBus;

    public RoleRepository(IOptions<RolesSettings> settings, ICapPublisher capBus)
    {
        _connectionString = settings.Value.ConnectionStrings.Database;
        _capBus = capBus;
    }

    public async Task<Role> GetAsync(Guid id)
    {
        var sql = $"SELECT * FROM Role WHERE Id = @id";

        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.QuerySingleOrDefaultAsync<Role>(sql, new { id });
        }
    }

    public async Task AddAsync(Role role)
    {
        var sql = @"INSERT INTO Role (Id, Name) VALUES (@id, @name)";

        var e = new RoleCreated(role.Id, role.Name);
        await SaveAndPublish(sql, new { id = role.Id, name = role.Name }, e);
    }

    public async Task AddPermissionToRoleAsync(Guid roleId, Guid permissionId)
    {
        var sql = @"INSERT INTO RolePermission (RoleId, PermissionId) VALUES (@roleId, @permissionId)";

        var e = new PermissionAddedToRole(roleId, permissionId);
        await SaveAndPublish(sql, new { roleId, permissionId }, e);
    }

    public async Task RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId)
    {
        var sql = @"DELETE FROM RolePermission WHERE RoleId = @roleId AND PermissionId = @permissionId";

        var e = new PermissionRemovedFromRole(roleId, permissionId);
        await SaveAndPublish(sql, new { roleId, permissionId }, e);
    }

    public async Task RemoveAsync(Guid id)
    {
        var sql = $"DELETE FROM Role WHERE Id = @id";

        var e = new RoleDeleted(id);
        await SaveAndPublish(sql, new { id }, e);
    }

    private async Task SaveAndPublish(string sql, object param, object e)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var transaction = connection.BeginTransaction(_capBus, autoCommit: false))
            {
                await connection.ExecuteAsync(sql, param, transaction);

                _capBus.Publish(e.GetType().Name, e);

                transaction.Commit();
            }
        }
    }
}
