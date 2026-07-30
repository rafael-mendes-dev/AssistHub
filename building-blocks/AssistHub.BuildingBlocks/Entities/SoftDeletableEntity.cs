namespace AssistHub.BuildingBlocks.Entities;

public abstract class SoftDeletableEntity : BaseEntity, ISoftDeletable
{
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
}
