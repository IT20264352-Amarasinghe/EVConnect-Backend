using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Slot
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("startTime")]
    public required DateTime StartTime { get; set; }

    [BsonElement("endTime")]
    public required DateTime EndTime { get; set; }

    [BsonElement("status")]
    public required string Status { get; set; } = "Available"; // Available, Booked, Unavailable
}