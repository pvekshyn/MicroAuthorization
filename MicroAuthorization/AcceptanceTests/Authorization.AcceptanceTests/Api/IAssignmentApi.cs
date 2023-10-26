using Refit;

namespace Authorization.AcceptanceTests;

[Headers("Authorization: Bearer")]
public interface IAssignmentApi
{
    [Get("/assignment/{id}")]
    Task<ApiResponse<string>> GetAsync(Guid id);

    [Post("/assign")]
    Task<ApiResponse<string>> AssignAsync(Assignment assignment);

    [Post("/deassign/role/{roleId}/user/{userId}")]
    Task<ApiResponse<string>> DeassignAsync(Guid roleId, Guid userId);
}
