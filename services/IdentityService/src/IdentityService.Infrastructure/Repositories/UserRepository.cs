using IdentityService.Domain.Entities;
using IdentityService.Domain.Repositories;
using IdentityService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Repositories;

public sealed class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .SingleOrDefaultAsync(user => user.Id == id, cancellationToken);
    } 

    public Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = User.NormalizeEmail(email);

        return dbContext.Users
            .SingleOrDefaultAsync(
                user => user.NormalizedEmail == normalizedEmail,
                cancellationToken);
    }

    public Task<User?> GetDeletedByEmailAsync(
        string email,
        DateTime deletedAfter,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = User.NormalizeEmail(email);

        return dbContext.Users
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(
                user => user.NormalizedEmail == normalizedEmail &&
                        user.IsDeleted &&
                        user.DeletedAt != null &&
                        user.DeletedAt > deletedAfter,
                cancellationToken);
    }

    public Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = User.NormalizeEmail(email);

        return dbContext.Users
            .IgnoreQueryFilters()
            .AnyAsync(
                user => user.NormalizedEmail == normalizedEmail,
                cancellationToken);
    }

    public void Add(User user)
    {
        dbContext.Users.Add(user);
    }

    public void SoftDelete(User user, DateTime deletedAt)
    {
        user.IsDeleted = true;
        user.DeletedAt = deletedAt;
        user.UpdatedAt = deletedAt;
    }

    public async Task<int> PermanentlyDeleteBeforeAsync(
        DateTime deletedBefore,
        int batchSize = 500,
        CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(batchSize);

        var expiredUserIds = await dbContext.Users
            .IgnoreQueryFilters()
            .Where(user =>
                user.IsDeleted &&
                user.DeletedAt != null &&
                user.DeletedAt <= deletedBefore)
            .OrderBy(user => user.DeletedAt)
            .Select(user => user.Id)
            .Take(batchSize)
            .ToListAsync(cancellationToken);

        if (expiredUserIds.Count == 0)
        {
            return 0;
        }

        return await dbContext.Users
            .IgnoreQueryFilters()
            .Where(user => expiredUserIds.Contains(user.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }
}
