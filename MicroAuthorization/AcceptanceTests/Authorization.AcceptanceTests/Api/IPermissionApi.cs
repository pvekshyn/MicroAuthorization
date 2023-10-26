using Refit;

namespace Authorization.AcceptanceTests;

[Headers("Authorization: Bearer")]
public interface IPermissionApi
{
    [Get("/permission/{id}")]
    Task<ApiResponse<string>> GetAsync(Guid id);

    [Post("/permission")]
    Task<ApiResponse<string>> CreateAsync(Permission Permission);

    [Delete("/permission/{id}")]
    Task<ApiResponse<string>> DeleteAsync(Guid id);
}
