using IdentityService.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityService.Infrastructure.Persistence;
using IdentityService.Infrastructure.Repositories;
using MongoDB.Driver;

namespace IdentityService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["MongoDb:ConnectionString"]
            ?? throw new InvalidOperationException("MongoDb:ConnectionString not found.");
        var databaseName = configuration["MongoDb:DatabaseName"]
            ?? throw new InvalidOperationException("MongoDb:DatabaseName not found.");

        MongoConfiguration.Configure();
        services.AddSingleton<IMongoClient>(new MongoClient(connectionString));
        services.AddSingleton(provider =>
            provider.GetRequiredService<IMongoClient>().GetDatabase(databaseName));
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddHostedService<MongoIndexInitializer>();

        return services;
    }
}
