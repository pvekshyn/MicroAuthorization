namespace Authorization.AcceptanceTests;

public record Permission(Guid Id, string Name);

public record Role(Guid Id, string Name);

public record Assignment(Guid Id, Guid RoleId, Guid UserId);

public record User(Guid Id, string FullName, string Email);
