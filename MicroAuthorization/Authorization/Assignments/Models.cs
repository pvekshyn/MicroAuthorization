namespace Assignments;

public record Assignment(Guid Id, Guid RoleId, Guid UserId);

//events
public record Assigned(Guid roleId, Guid userId);
public record Deassigned(Guid roleId, Guid userId);