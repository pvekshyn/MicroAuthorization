namespace Authorization.EventHandler;

public class AuthorizationSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
}

public class ConnectionStrings
{
    public string Database { get; set; }
}
