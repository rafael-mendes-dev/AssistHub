using IdentityService.Domain.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace IdentityService.Infrastructure.Persistence;

public static class MongoConfiguration
{
    private static bool _configured;

    public static void Configure()
    {
        if (_configured) return;
        _configured = true;

        ConventionRegistry.Register("camelCase", new ConventionPack { new CamelCaseElementNameConvention() }, _ => true);
        BsonSerializer.TryRegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonSerializer.TryRegisterSerializer(new ProviderKeySerializer());
    }
}