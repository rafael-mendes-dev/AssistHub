namespace IdentityService.Domain.ValueObjects;

public readonly struct ProviderKey : IEquatable<ProviderKey>
{
    public string Value { get; }

    public ProviderKey(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value.Trim().ToLowerInvariant();
    }

    public static readonly ProviderKey MetaCloudApi = new("meta-cloud-api");
    public static readonly ProviderKey EvolutionApi = new("evolution-api");
    public static readonly ProviderKey WebChat = new("web-chat");

    public bool Equals(ProviderKey other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is ProviderKey other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(ProviderKey key) => key.Value;
    public static explicit operator ProviderKey(string value) => new(value);
}
