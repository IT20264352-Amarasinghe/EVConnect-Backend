// this is used to send booking details with the charger and slot details
public class BookingDto
{
    public string? Id { get; set; }
    public string CustomerNic { get; set; }
    public Charger Charger { get; set; }
    public Slot Slot { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Status { get; set; }
}