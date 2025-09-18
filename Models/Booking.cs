using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Booking
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("customerNic")]
    public required string CustomerNic { get; set; }

    [BsonElement("chargerId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string ChargerId { get; set; }

    [BsonElement("slotId")]
    public required string SlotId { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("status")]
    public string? Status { get; set; } = "Active"; // Active, Cancelled, Updated
}