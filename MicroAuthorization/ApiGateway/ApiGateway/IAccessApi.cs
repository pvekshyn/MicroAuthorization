﻿using Refit;

namespace ApiGateway;

public interface IAccessApi
{
    [Get("/check-access/user/{userId}/permission/{permissionId}")]
    Task<ApiResponse<string>> CheckAccessAsync(Guid userId, Guid permissionId);
}
