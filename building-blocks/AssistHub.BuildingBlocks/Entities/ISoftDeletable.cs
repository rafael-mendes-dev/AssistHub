namespace AssistHub.BuildingBlocks.Entities;

public interface ISoftDeletable
{
    DateTime? DeletedAt { get; set; }
    bool IsDeleted { get; set; }
}
