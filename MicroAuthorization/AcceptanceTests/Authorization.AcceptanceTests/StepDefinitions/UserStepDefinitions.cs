using Refit;

namespace Authorization.AcceptanceTests.StepDefinitions;

[Binding]
public class UserStepDefinitions
{
    private ScenarioContext _scenarioContext;
    private IUserApi _userApiClient;

    public UserStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;

        var token = _scenarioContext.Get<string>("accessToken");
        var settings = new RefitSettings
        {
            AuthorizationHeaderValueGetter = () => Task.FromResult(token),
            ExceptionFactory = httpResponse => Task.FromResult<Exception>(null)
        };

        _userApiClient = RestService.For<IUserApi>(Constants.UserUrl, settings);
    }

    [When(@"create user (.*) with email (.*)")]
    public async Task WhenCreateUserWithEmail(string name, string email)
    {
        var user = new User(Guid.NewGuid(), name, email);
        _scenarioContext.Set(user, name);

        var result = await _userApiClient.CreateAsync(user);

        _scenarioContext.Set(result.StatusCode);
    }


    [When(@"get user (.*)")]
    public async Task WhenGetUser(User user)
    {
        var result = await _userApiClient.GetAsync(user.Id);

        _scenarioContext.Set(result.StatusCode);
    }

    [When(@"delete user (.*)")]
    public async Task WhenDeleteUser(User user)
    {
        var result = await _userApiClient.DeleteAsync(user.Id);

        _scenarioContext.Set(result.StatusCode);
    }
}
