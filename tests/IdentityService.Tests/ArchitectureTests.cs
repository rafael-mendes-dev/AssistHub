using AssistHub.BuildingBlocks.Entities;

namespace IdentityService.Tests;

public class ArchitectureTests
{
    [Fact]
    public void DomainBuildingBlocksDoNotDependOnMongoDriver()
    {
        var references = typeof(BaseEntity).Assembly.GetReferencedAssemblies();

        Assert.DoesNotContain(references, reference => reference.Name == "MongoDB.Driver");
    }
}
