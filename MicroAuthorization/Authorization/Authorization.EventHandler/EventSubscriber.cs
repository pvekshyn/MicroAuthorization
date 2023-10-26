using DotNetCore.CAP;

namespace Authorization.EventHandler;
public class EventSubscriber : ICapSubscribe
{
    private readonly IUserRepository _userRepository;

    public EventSubscriber(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [CapSubscribe("UserCreated")]
    public async Task CreateUserAsync(UserCreated e)
    {
        await _userRepository.AddUserAsync(e.Id, e.FullName, e.Email);
    }

    [CapSubscribe("UserDeleted")]
    public async Task DeleteUserAsync(UserDeleted e)
    {
        await _userRepository.RemoveUserAsync(e.Id);
    }
}
