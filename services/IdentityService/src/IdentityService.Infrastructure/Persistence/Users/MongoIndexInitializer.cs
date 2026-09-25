using IdentityService.Domain.Features.Users;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace IdentityService.Infrastructure.Persistence.Users;

public sealed class MongoIndexInitializer(IMongoDatabase database) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var users = database.GetCollection<User>(MongoCollections.Users);

        return users.Indexes.CreateOneAsync(
            new CreateIndexModel<User>(
                Builders<User>.IndexKeys.Ascending(user => user.EmailNormalized),
                new CreateIndexOptions { Unique = true }),
            cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
