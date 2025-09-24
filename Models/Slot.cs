using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

// this contain all the attribute for Slots
public class Slot
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("chargerId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ChargerId { get; set; } = default!;

    [BsonElement("date")]
    public required DateTime Date { get; set; }

    [BsonElement("startTime")]
    public required TimeSpan StartTime { get; set; }

    [BsonElement("endTime")]
    public required TimeSpan EndTime { get; set; }

    [BsonElement("status")]
    public required string Status { get; set; } = "Available"; // Available, Booked, Unavailable
}