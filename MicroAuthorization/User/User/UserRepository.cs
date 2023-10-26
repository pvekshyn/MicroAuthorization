using DotNetCore.CAP;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Users;

public interface IUserRepository
{
    public Task<User> GetAsync(Guid id);
    public Task AddAsync(User assignment);
    public Task RemoveAsync(Guid id);
}

public class UserRepository : IUserRepository
{
    private string _connectionString { get; init; }
    private readonly ICapPublisher _capBus;
    private MongoClient _client;
    private IMongoCollection<User> _userCollection;

    public UserRepository(IOptions<UserSettings> settings, ICapPublisher capBus)
    {
        _connectionString = settings.Value.ConnectionStrings.Database;
        _capBus = capBus;

        _client = new MongoClient(_connectionString);
        var database = _client.GetDatabase("User");
        _userCollection = database.GetCollection<User>("Users");
    }

    public async Task<User> GetAsync(Guid id)
    {
        var filter = Builders<User>.Filter.Eq("_id", id);

        return await _userCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task AddAsync(User user)
    {
        using (var session = _client.StartTransaction(_capBus, autoCommit: false))
        {
            await _userCollection.InsertOneAsync(session, user);

            var e = new UserCreated(user.Id, user.FirstName, user.LastName, user.FullName, user.Email);
            _capBus.Publish(e.GetType().Name, e);

            session.CommitTransaction();
        }
    }

    public async Task RemoveAsync(Guid id)
    {
        using (var session = _client.StartTransaction(_capBus, autoCommit: false))
        {
            await _userCollection.DeleteOneAsync(x => x.Id == id);

            var e = new UserDeleted(id);
            _capBus.Publish(e.GetType().Name, e);

            session.CommitTransaction();
        }
    }
}
