using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

// this contain all the attribute for Charger
public class Charger
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("code")]
    public required string Code { get; set; }

    [BsonElement("location")]
    public required string Location { get; set; }

}