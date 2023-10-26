using Microsoft.AspNetCore.Mvc.Testing;
using Refit;
using System.Net;

namespace Access.Tests.Integration;

public class AccessTests
{
    private readonly IAccessApi _accessApi;

    public AccessTests()
    {
        var apiFactory = new WebApplicationFactory<Program>();

        var settings = new RefitSettings
        {
            ExceptionFactory = httpResponse => Task.FromResult<Exception>(null)
        };

        var httpClient = apiFactory.CreateClient();
        _accessApi = RestService.For<IAccessApi>(httpClient, settings);
    }

    [Fact]
    public async Task AccessTest()
    {
        var result = await _accessApi.CheckAccessAsync(Guid.NewGuid(), Guid.NewGuid());
        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
    }
}