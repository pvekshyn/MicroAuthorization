using NUnit.Framework;
using Refit;
using System.Net;

namespace Authorization.AcceptanceTests.StepDefinitions;
[Binding]
public sealed class AuthorizationStepDefinitions
{
    private ScenarioContext _scenarioContext;
    private IPermissionApi _permissionApiClient;
    private IRoleApi _roleApiClient;
    private IAssignmentApi _assignmentApiClient;
    private IAccessApi _accessApiClient;
    private IUserApi _userApiClient;

    public AuthorizationStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;

        var token = _scenarioContext.Get<string>("accessToken");
        var settings = new RefitSettings
        {
            AuthorizationHeaderValueGetter = () => Task.FromResult(token),
            ExceptionFactory = httpResponse => Task.FromResult<Exception>(null)
        };

        _roleApiClient = RestService.For<IRoleApi>(Constants.RoleUrl, settings);
        _permissionApiClient = RestService.For<IPermissionApi>(Constants.RoleUrl, settings);
        _assignmentApiClient = RestService.For<IAssignmentApi>(Constants.AssignmentUrl, settings);
        _accessApiClient = RestService.For<IAccessApi>(Constants.AccessUrl, settings);
        _userApiClient = RestService.For<IUserApi>(Constants.UserUrl, settings);
    }

    [Given(@"permission (.*) created")]
    public async Task GivenPermissionCreated(string name)
    {
        var permission = new Permission(Guid.NewGuid(), name);
        _scenarioContext.Set(permission, name);

        var result = await _permissionApiClient.CreateAsync(permission);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
    }

    [Given(@"role (.*) created")]
    public async Task WhenCreateRole(string name)
    {
        var role = new Role(Guid.NewGuid(), name);
        _scenarioContext.Set(role, name);

        var result = await _roleApiClient.CreateAsync(role);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
    }

    [Given(@"user (.*) created")]
    public async Task WhenUserCreated(string name)
    {
        var user = new User(Guid.NewGuid(), name, $"{name}@gmail.com");
        _scenarioContext.Set(user, name);

        var result = await _userApiClient.CreateAsync(user);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
    }


    [Given(@"permission (.*) added to role (.*)")]
    public async Task WhenAddPermissionToRole(Permission permission, Role role)
    {
        var result = await _roleApiClient.AddPermissionToRoleAsync(role.Id, permission.Id);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
    }

    [When(@"create permission (.*)")]
    public async Task WhenPermissionCreated(string name)
    {
        var permission = new Permission(Guid.NewGuid(), name);

        var result = await _permissionApiClient.CreateAsync(permission);

        _scenarioContext.Set(result.StatusCode);
    }

    [When(@"user (.*) assigned to role (.*)")]
    public async Task WhenAssignUser(User user, Role role)
    {
        var assignment = new Assignment(Guid.NewGuid(), role.Id, user.Id);

        var result = await _assignmentApiClient.AssignAsync(assignment);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
    }

    [When(@"user (.*) deassigned from role (.*)")]
    public async Task WhenDeassignUser(User user, Role role)
    {
        var result = await _assignmentApiClient.DeassignAsync(role.Id, user.Id);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
    }

    [Then(@"user (.*) has (.*) permission")]
    public async Task UserHasPermission(User user, Permission permission)
    {
        var result = await _accessApiClient.CheckAccessAsync(user.Id, permission.Id);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
    }

    [Then(@"user (.*) doesn't have (.*) permission")]
    public async Task UserDoesnotHavePermission(User user, Permission permission)
    {
        var result = await _accessApiClient.CheckAccessAsync(user.Id, permission.Id);

        Assert.AreEqual(HttpStatusCode.Forbidden, result.StatusCode);
    }

    [Then(@"result (.*)")]
    public void ThenResult(string statusCode)
    {
        var actualStatusCode = _scenarioContext.Get<HttpStatusCode>();
        Assert.AreEqual(statusCode, actualStatusCode.ToString());
    }
}
