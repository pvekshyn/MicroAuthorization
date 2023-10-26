using Refit;

namespace Roles.Tests.Integration;
public interface IPermissionApi
{
    [Get("/permission/{id}")]
    Task<ApiResponse<string>> GetAsync(Guid id);

    [Post("/permission")]
    Task<ApiResponse<string>> CreateAsync(Permission Permission);

    [Delete("/permission/{id}")]
    Task<ApiResponse<string>> DeleteAsync(Guid id);
}
