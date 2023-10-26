using Microsoft.AspNetCore.Mvc.Testing;
using Refit;
using System.Net;

namespace Roles.Tests.Integration;

public class PermissionTests
{
    private readonly IPermissionApi _permissionApi;

    public PermissionTests()
    {
        var apiFactory = new WebApplicationFactory<Program>();

        var settings = new RefitSettings
        {
            ExceptionFactory = httpResponse => Task.FromResult<Exception>(null)
        };

        var httpClient = apiFactory.CreateClient();
        _permissionApi = RestService.For<IPermissionApi>(httpClient, settings);
    }

    [Fact]
    public async Task PermissionTest()
    {
        var permission = new Permission(Guid.NewGuid(), $"test permission {Guid.NewGuid()}");

        var result = await _permissionApi.CreateAsync(permission);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        result = await _permissionApi.GetAsync(permission.Id);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        result = await _permissionApi.DeleteAsync(permission.Id);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        result = await _permissionApi.GetAsync(permission.Id);
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}