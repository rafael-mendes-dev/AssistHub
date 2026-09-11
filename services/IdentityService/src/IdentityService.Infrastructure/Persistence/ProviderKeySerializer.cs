using IdentityService.Domain.ValueObjects;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace IdentityService.Infrastructure.Persistence;

public sealed class ProviderKeySerializer : SerializerBase<ProviderKey>
{
    public override ProviderKey Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args) =>
        new(context.Reader.ReadString());

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ProviderKey value) =>
        context.Writer.WriteString(value.Value);
}
