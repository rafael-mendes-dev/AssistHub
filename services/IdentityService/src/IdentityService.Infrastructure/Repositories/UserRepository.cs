using AssistHub.BuildingBlocks.Persistence;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using MongoDB.Driver;

namespace IdentityService.Infrastructure.Repositories;

public sealed class UserRepository : MongoRepository<User>, IUserRepository
{
    public UserRepository(IMongoDatabase database) : base(database, "users")
    {
        Collection.Indexes.CreateMany([
            new CreateIndexModel<User>(
                Builders<User>.IndexKeys.Ascending(user => user.NormalizedEmail),
                new CreateIndexOptions { Unique = true }),
            new CreateIndexModel<User>(
                Builders<User>.IndexKeys.Ascending(user => user.DeletedAt))
        ]);
    }

    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = User.NormalizeEmail(email);

        return await Collection.Find(user =>
                user.NormalizedEmail == normalizedEmail && !user.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetDeletedByEmailAsync(
        string email,
        DateTime deletedAfter,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = User.NormalizeEmail(email);

        return await Collection.Find(user =>
                user.NormalizedEmail == normalizedEmail &&
                user.IsDeleted &&
                user.DeletedAt != null &&
                user.DeletedAt > deletedAfter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = User.NormalizeEmail(email);

        return Collection.Find(user => user.NormalizedEmail == normalizedEmail)
            .AnyAsync(cancellationToken);
    }

    public Task SoftDeleteAsync(
        User user,
        DateTime deletedAt,
        CancellationToken cancellationToken = default)
    {
        user.IsDeleted = true;
        user.DeletedAt = deletedAt;
        user.UpdatedAt = deletedAt;
        return UpdateAsync(user, cancellationToken);
    }

    public async Task<int> PermanentlyDeleteBeforeAsync(
        DateTime deletedBefore,
        int batchSize = 500,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(batchSize);

        var expiredUserIds = await Collection.Find(user =>
                user.IsDeleted &&
                user.DeletedAt != null &&
                user.DeletedAt <= deletedBefore)
            .SortBy(user => user.DeletedAt)
            .Limit(batchSize)
            .Project(user => user.Id)
            .ToListAsync(cancellationToken);

        if (expiredUserIds.Count == 0)
        {
            return 0;
        }

        var result = await Collection.DeleteManyAsync(
            user => expiredUserIds.Contains(user.Id),
            cancellationToken);

        return checked((int)result.DeletedCount);
    }
}
