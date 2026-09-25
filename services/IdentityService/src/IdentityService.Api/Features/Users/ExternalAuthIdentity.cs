
namespace IdentityService.Features.Users;

public class ExternalAuthIdentity
{
    public required ProviderKey Provider { get; set; }
    public required string Subject { get; set; }
    public string? ProviderEmail { get; set; }
    public DateTime LinkedAt { get; set; } = DateTime.UtcNow;
}
