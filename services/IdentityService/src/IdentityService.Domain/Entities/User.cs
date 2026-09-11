using AssistHub.BuildingBlocks.Entities;
using IdentityService.Domain.Enums;

namespace IdentityService.Domain.Entities;

public class User : BaseEntity, IAuditable
{
    public required string Email { get; set { field = value.Trim(); EmailNormalized = NormalizeEmail(value); } } = null!;
    public string EmailNormalized { get; private set; } = null!;
    public required string DisplayName { get; set; }
    public required UserStatusEnum Status { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? PhoneNumber { get; set; }
    public List<ExternalAuthIdentity> ExternalAuth { get; set; } = [];


    public static string NormalizeEmail(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        return email.Trim().ToUpperInvariant();
    }
}
