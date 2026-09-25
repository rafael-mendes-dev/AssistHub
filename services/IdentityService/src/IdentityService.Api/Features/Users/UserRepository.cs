using AssistHub.BuildingBlocks.Persistence;
using MongoDB.Driver;

namespace IdentityService.Features.Users;

public sealed class UserRepository(IMongoDatabase database) : MongoRepository<User>(database, MongoCollections.Users), IUserRepository
{

    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = User.NormalizeEmail(email);

        return await Collection.Find(user =>
                user.EmailNormalized == normalizedEmail)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = User.NormalizeEmail(email);

        return Collection.Find(user => user.EmailNormalized == normalizedEmail)
            .AnyAsync(cancellationToken);
    }
}
