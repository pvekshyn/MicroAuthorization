using Refit;

namespace Authorization.AcceptanceTests;

[Headers("Authorization: Bearer")]
public interface IUserApi
{
    [Get("/user/{id}")]
    Task<ApiResponse<string>> GetAsync(Guid id);

    [Post("/user")]
    Task<ApiResponse<string>> CreateAsync(User user);

    [Delete("/user/{id}")]
    Task<ApiResponse<string>> DeleteAsync(Guid id);
}
