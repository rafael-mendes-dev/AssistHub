using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<User?> GetDeletedByEmailAsync(
        string email,
        DateTime deletedAfter,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    void Add(User user);

    void SoftDelete(User user, DateTime deletedAt);

    Task<int> PermanentlyDeleteBeforeAsync(
        DateTime deletedBefore,
        int batchSize = 500,
        CancellationToken cancellationToken = default);
}
