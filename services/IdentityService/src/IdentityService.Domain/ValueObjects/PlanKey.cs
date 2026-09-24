namespace IdentityService.Domain.ValueObjects;

public readonly record struct PlanKey
{
    public string Value { get;}

    public PlanKey(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value.Trim().ToLowerInvariant();
    }

    public static implicit operator string(PlanKey key) => key.Value;
    public static explicit operator PlanKey(string value) => new(value);

    public override string ToString() => Value;
}