namespace Authorization.EventHandler;

public record UserCreated(Guid Id, string FullName, string Email);
public record UserDeleted(Guid Id);
