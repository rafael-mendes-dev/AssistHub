using AssistHub.BuildingBlocks.Entities;

namespace IdentityService.Domain.Entities;

public class User : AuditableSoftDeletableEntity
{
    private string _email = null!;

    public required string FirstName { get; set; }
    public required string PasswordHash { get; set; }
    public required string LastName { get; set; }

    public required string Email
    {
        get => _email;
        set
        {
            _email = value.Trim();
            NormalizedEmail = NormalizeEmail(value);
        }
    }

    public string NormalizedEmail { get; private set; } = null!;
    public string? PhoneNumber { get; set; }

    public static string NormalizeEmail(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        return email.Trim().ToUpperInvariant();
    }
}
