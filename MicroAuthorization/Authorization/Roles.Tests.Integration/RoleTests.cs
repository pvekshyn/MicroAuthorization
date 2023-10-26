using Microsoft.AspNetCore.Mvc.Testing;
using Refit;
using System.Net;

namespace Roles.Tests.Integration;

public class RoleTests
{
    private readonly IPermissionApi _permissionApi;
    private readonly IRoleApi _roleApi;


    public RoleTests()
    {
        var apiFactory = new WebApplicationFactory<Program>();

        var settings = new RefitSettings
        {
            ExceptionFactory = httpResponse => Task.FromResult<Exception>(null)
        };

        var httpClient = apiFactory.CreateClient();
        _permissionApi = RestService.For<IPermissionApi>(httpClient, settings);
        _roleApi = RestService.For<IRoleApi>(httpClient, settings);
    }

    [Fact]
    public async Task RoleTest()
    {
        var permission = new Permission(Guid.NewGuid(), $"test permission {Guid.NewGuid()}");
        var role = new Role(Guid.NewGuid(), $"test role {Guid.NewGuid()}");

        var result = await _roleApi.CreateAsync(role);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        result = await _roleApi.GetAsync(role.Id);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        result = await _permissionApi.CreateAsync(permission);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        result = await _roleApi.AddPermissionToRoleAsync(role.Id, permission.Id);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        result = await _roleApi.RemovePermissionFromRoleAsync(role.Id, permission.Id);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        result = await _roleApi.DeleteAsync(role.Id);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        result = await _roleApi.GetAsync(role.Id);
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}