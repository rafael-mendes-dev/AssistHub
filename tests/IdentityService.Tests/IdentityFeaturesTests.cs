using IdentityService.Domain.Features.Tenants;
using IdentityService.Domain.Features.Users;

namespace IdentityService.Tests;

public class IdentityFeaturesTests
{
    [Theory]
    [InlineData("  Alice@Example.com  ", "ALICE@EXAMPLE.COM")]
    [InlineData("bob@example.com", "BOB@EXAMPLE.COM")]
    public void UserEmailNormalizationPreservesExistingBehavior(string email, string expected)
    {
        Assert.Equal(expected, User.NormalizeEmail(email));
    }

    [Fact]
    public void ProviderKeyIsCaseInsensitiveAfterNormalization()
    {
        Assert.Equal(new ProviderKey(" Meta-Cloud-API "), ProviderKey.MetaCloudApi);
    }

    [Fact]
    public void PlanKeyNormalizesItsValue()
    {
        Assert.Equal("starter", new PlanKey(" Starter ").Value);
    }

    [Fact]
    public void TenantSettingsRejectInvalidTimeZone()
    {
        Assert.Throws<TimeZoneNotFoundException>(() => new TenantSettings("pt-BR", "Not/A_Time_Zone"));
    }
}
