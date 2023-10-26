using Refit;

namespace Authorization.AcceptanceTests;
[Binding]
public class Hooks
{
    private ScenarioContext _scenarioContext;
    private IPermissionApi _permissionApiClient;
    private IRoleApi _roleApiClient;
    private IUserApi _userApiClient;

    public Hooks(ScenarioContext scenarioContext)
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
        _userApiClient = RestService.For<IUserApi>(Constants.UserUrl, settings);
    }

    [AfterScenario("Cleanup")]
    public async Task Cleanup()
    {
        var role = _scenarioContext.Get<Role>("Test Admin");
        await _roleApiClient.DeleteAsync(role.Id);

        var permission = _scenarioContext.Get<Permission>("Read Test");
        await _permissionApiClient.DeleteAsync(permission.Id);

        var user = _scenarioContext.Get<User>("John");
        await _userApiClient.DeleteAsync(user.Id);
    }
}
