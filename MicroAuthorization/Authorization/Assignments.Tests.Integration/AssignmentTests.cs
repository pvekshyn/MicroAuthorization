using Microsoft.AspNetCore.Mvc.Testing;
using Refit;
using System.Net;

namespace Assignments.Tests.Integration;

public class AssignmentTests
{
    private readonly IAssignmentApi _assignmentApi;

    private readonly Guid testRoleId = new Guid("d69c8657-adbc-4da1-aa71-eaa430c094ef");
    private readonly Guid testUserId = new Guid("68D76B61-2532-4AA9-BC68-733E65D878B9");

    public AssignmentTests()
    {
        var apiFactory = new WebApplicationFactory<Program>();

        var settings = new RefitSettings
        {
            ExceptionFactory = httpResponse => Task.FromResult<Exception>(null)
        };

        var httpClient = apiFactory.CreateClient();
        _assignmentApi = RestService.For<IAssignmentApi>(httpClient, settings);
    }

    [Fact]
    public async Task AssignmentTest()
    {
        var assignment = new Assignment(Guid.NewGuid(), testRoleId, testUserId);

        var result = await _assignmentApi.AssignAsync(assignment);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        result = await _assignmentApi.GetAsync(assignment.RoleId, assignment.UserId);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        result = await _assignmentApi.DeassignAsync(assignment.RoleId, assignment.UserId);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        result = await _assignmentApi.GetAsync(assignment.RoleId, assignment.UserId);
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}