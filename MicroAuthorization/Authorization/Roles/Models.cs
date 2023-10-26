namespace Roles;

public record Permission(Guid Id, string Name);
public record Role(Guid Id, string Name);

//events
public record RoleCreated(Guid roleId, string Name);
public record RoleDeleted(Guid roleId);
public record PermissionAddedToRole(Guid roleId, Guid permissionId);
public record PermissionRemovedFromRole(Guid roleId, Guid permissionId);