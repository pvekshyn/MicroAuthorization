namespace Users;

public record User(Guid Id, string FirstName, string LastName, string FullName, string Email);

//events
public record UserCreated(Guid Id, string FirstName, string LastName, string FullName, string Email);
public record UserDeleted(Guid Id);
