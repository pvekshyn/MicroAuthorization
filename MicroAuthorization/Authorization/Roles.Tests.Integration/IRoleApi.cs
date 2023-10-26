using Refit;

namespace Roles.Tests.Integration;
public interface IRoleApi
{
    [Get("/role/{id}")]
    Task<ApiResponse<string>> GetAsync(Guid id);

    [Post("/role")]
    Task<ApiResponse<string>> CreateAsync(Role role);

    [Put("/role/{id}/add/permission/{permissionId}")]
    Task<ApiResponse<string>> AddPermissionToRoleAsync(Guid id, Guid permissionId);

    [Put("/role/{id}/remove/permission/{permissionId}")]
    Task<ApiResponse<string>> RemovePermissionFromRoleAsync(Guid id, Guid permissionId);

    [Delete("/role/{id}")]
    Task<ApiResponse<string>> DeleteAsync(Guid id);
}
