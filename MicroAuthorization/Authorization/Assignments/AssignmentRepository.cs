using Dapper;
using DotNetCore.CAP;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Assignments;

public interface IAssignmentRepository
{
    public Task<Assignment> GetAsync(Guid roleId, Guid userId);
    public Task AddAsync(Assignment assignment);
    public Task RemoveAsync(Guid roleId, Guid userId);
}

public class AssignmentRepository : IAssignmentRepository
{
    private string _connectionString { get; init; }
    private readonly ICapPublisher _capBus;

    public AssignmentRepository(IOptions<AssignmentSettings> settings, ICapPublisher capBus)
    {
        _connectionString = settings.Value.ConnectionStrings.Database;
        _capBus = capBus;
    }

    public async Task<Assignment> GetAsync(Guid roleId, Guid userId)
    {
        var sql = $"SELECT * FROM Assignment WHERE RoleId = @roleId AND UserId = @userId";

        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.QuerySingleOrDefaultAsync<Assignment>(sql, new { roleId, userId });
        }
    }

    public async Task AddAsync(Assignment assignment)
    {
        var sql = $"INSERT INTO Assignment (Id, RoleId, UserId) VALUES (@id, @roleId, @userId)";

        var e = new Assigned(assignment.RoleId, assignment.UserId);

        await SaveAndPublish(sql, new { id = assignment.Id, roleId = assignment.RoleId, userId = assignment.UserId }, e);
    }

    public async Task RemoveAsync(Guid roleId, Guid userId)
    {
        var sql = $"DELETE FROM Assignment WHERE RoleId = @roleId AND UserId = @userId";

        var e = new Deassigned(roleId, userId);

        await SaveAndPublish(sql, new { roleId, userId }, e);
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
