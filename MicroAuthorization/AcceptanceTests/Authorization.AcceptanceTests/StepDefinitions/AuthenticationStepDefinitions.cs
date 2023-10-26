using IdentityModel.Client;

namespace Authorization.AcceptanceTests.StepDefinitions;
[Binding]
public class AuthenticationStepDefinitions
{
    private ScenarioContext _scenarioContext;

    public AuthenticationStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"I am not logged in")]
    public async Task NotLoggedIn()
    {
        _scenarioContext["accessToken"] = string.Empty;
    }

    [Given(@"I am logged in as admin")]
    public async Task LoggedInAsAdmin()
    {
        _scenarioContext["accessToken"] = await GetAccessTokenAsync("admin");
    }

    [Given(@"I am logged in as user without permissions")]
    public async Task LoggedInAsUser()
    {
        _scenarioContext["accessToken"] = await GetAccessTokenAsync("user");
    }

    public static async Task<string> GetAccessTokenAsync(string clientId)
    {
        var client = new HttpClient();
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = $"{Constants.IdentityServerUrl}/connect/token",
            ClientId = clientId,
            ClientSecret = "secret",
            Scope = "api"

        });
        tokenResponse.HttpResponse.EnsureSuccessStatusCode();

        return tokenResponse.AccessToken;
    }
}
