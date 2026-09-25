using AssistHub.BuildingBlocks.Entities;
using AssistHub.BuildingBlocks.Persistence;
using IdentityService.Domain.Features.Users;

namespace IdentityService.Tests;

public class ArchitectureTests
{
    [Fact]
    public void DomainBuildingBlocksDoNotDependOnMongoDriver()
    {
        var references = typeof(BaseEntity).Assembly.GetReferencedAssemblies();

        Assert.DoesNotContain(references, reference => reference.Name == "MongoDB.Driver");
    }

    [Fact]
    public void MongoRepositoryIsProvidedBySharedPersistenceAssembly()
    {
        Assert.Equal("AssistHub.BuildingBlocks.Persistence", typeof(MongoRepository<User>).Assembly.GetName().Name);
    }
}
