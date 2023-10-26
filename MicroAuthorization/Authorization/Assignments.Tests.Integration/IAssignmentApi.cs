using Refit;

namespace Assignments.Tests.Integration;
public interface IAssignmentApi
{
    [Get("/assignment/role/{roleId}/user/{userId}")]
    Task<ApiResponse<string>> GetAsync(Guid roleId, Guid userId);

    [Post("/assign")]
    Task<ApiResponse<string>> AssignAsync(Assignment assignment);

    [Post("/deassign/role/{roleId}/user/{userId}")]
    Task<ApiResponse<string>> DeassignAsync(Guid roleId, Guid userId);
}
