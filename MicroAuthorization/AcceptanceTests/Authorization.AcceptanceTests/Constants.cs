namespace Authorization.AcceptanceTests;
internal class Constants
{
    public const string IdentityServerUrl = "http://localhost:5001";

    public const string AuthApiGatewayUrl = "http://localhost:8081";
    public const string RoleUrl = $"{AuthApiGatewayUrl}/role-api";
    public const string AssignmentUrl = $"{AuthApiGatewayUrl}/assignment-api";
    public const string AccessUrl = $"{AuthApiGatewayUrl}/access-api";

    public const string UserApiGatewayUrl = "http://localhost:8082";
    public const string UserUrl = $"{UserApiGatewayUrl}/user-api";
}
