using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Charger
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("code")]
    public required string Code { get; set; }

    [BsonElement("location")]
    public required string Location { get; set; }

    [BsonElement("slots")]
    public List<Slot> Slots { get; set; } = new();
}