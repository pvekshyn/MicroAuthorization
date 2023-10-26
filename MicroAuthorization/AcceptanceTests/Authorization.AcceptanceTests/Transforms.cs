namespace Authorization.AcceptanceTests;
[Binding]
public class Transforms
{
    private ScenarioContext _scenarioContext;

    public Transforms(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [StepArgumentTransformation]
    public Permission PermissionTransform(string name)
    {
        return _scenarioContext.Get<Permission>(name);
    }

    [StepArgumentTransformation]
    public Role RoleTransform(string name)
    {
        return _scenarioContext.Get<Role>(name);
    }

    [StepArgumentTransformation]
    public User UserTransform(string name)
    {
        return _scenarioContext.Get<User>(name);
    }
}
