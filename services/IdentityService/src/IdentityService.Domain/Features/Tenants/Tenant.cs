using AssistHub.BuildingBlocks.Entities;

namespace IdentityService.Domain.Features.Tenants;

public class Tenant : BaseEntity, IAuditable
{
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public TenantStatusEnum Status { get; set; }
    public PlanKey PlanKey { get; set; }
    public TenantSettings Settings { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}