namespace IdentityService.Domain.ValueObjects;

public readonly record struct TenantSettings
{
    public string Locale { get; }
    public string TimeZone { get; }

    public TenantSettings(string locale, string timeZone)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(locale);
        ArgumentException.ThrowIfNullOrWhiteSpace(timeZone);

        Locale = locale.Trim();
        TimeZone = timeZone.Trim();

        _ = TimeZoneInfo.FindSystemTimeZoneById(TimeZone);
    }
}
