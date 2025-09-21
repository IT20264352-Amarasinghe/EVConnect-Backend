// this is used to send booking details with the charger and slot details
public class BookingDto
{
    public string? Id { get; set; }
    public required string CustomerNic { get; set; }
    public required Charger Charger { get; set; }
    public required Slot Slot { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public required string Status { get; set; }
}