using AssistHub.BuildingBlocks.Entities;
using MongoDB.Driver;

namespace AssistHub.BuildingBlocks.Persistence;

public abstract class MongoRepository<TDocument>(
    IMongoDatabase database,
    string collectionName)
    where TDocument : BaseEntity
{
    protected IMongoCollection<TDocument> Collection { get; } = database.GetCollection<TDocument>(collectionName);

    public async Task<TDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await Collection.Find(document => document.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<List<TDocument>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Collection.Find(Builders<TDocument>.Filter.Empty)
            .ToListAsync(cancellationToken);

    public Task AddAsync(TDocument document, CancellationToken cancellationToken = default)
    {
        if (document is IAuditable auditable)
            auditable.CreatedAt = DateTime.UtcNow;

        return Collection.InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    public async Task<bool> UpdateAsync(TDocument document, CancellationToken cancellationToken = default)
    {
        if (document is IAuditable auditable)
            auditable.UpdatedAt = DateTime.UtcNow;

        var result = await Collection.ReplaceOneAsync(
            current => current.Id == document.Id,
            document,
            cancellationToken: cancellationToken);
        
        return result.IsAcknowledged && result.MatchedCount > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await Collection.DeleteOneAsync(
            x => x.Id == id,
            cancellationToken);
        
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}