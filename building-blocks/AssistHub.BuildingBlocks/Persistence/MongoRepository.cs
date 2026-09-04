using AssistHub.BuildingBlocks.Entities;
using MongoDB.Driver;

namespace AssistHub.BuildingBlocks.Persistence;

public abstract class MongoRepository<TDocument>(
    IMongoDatabase database,
    string collectionName)
    where TDocument : BaseEntity
{
    protected IMongoCollection<TDocument> Collection { get; } =
        database.GetCollection<TDocument>(collectionName);

    public async Task<TDocument?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default) =>
        await Collection.Find(document => document.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<List<TDocument>> GetAllAsync(
        CancellationToken cancellationToken = default) =>
        Collection.Find(Builders<TDocument>.Filter.Empty)
            .ToListAsync(cancellationToken);

    public Task AddAsync(
        TDocument document,
        CancellationToken cancellationToken = default) =>
        Collection.InsertOneAsync(document, cancellationToken: cancellationToken);

    public Task UpdateAsync(
        TDocument document,
        CancellationToken cancellationToken = default) =>
        Collection.ReplaceOneAsync(
            current => current.Id == document.Id,
            document,
            cancellationToken: cancellationToken);

    public Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default) =>
        Collection.DeleteOneAsync(
            document => document.Id == id,
            cancellationToken);
}
