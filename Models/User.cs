using MongoDB.Bson.Serialization.Attributes;

// this contain all the attribute for User
namespace EVConnectService.Models
{
    public class User
    {
        [BsonId]  // This replaces MongoDB's _id with NIC
        [BsonElement("_id")]
        public required string NIC { get; set; }

        [BsonElement("name")]
        public required string Name { get; set; }

        [BsonElement("email")]
        public required string Email { get; set; }

        [BsonElement("phone")]
        public required string Phone { get; set; }

        [BsonElement("role")]
        public required string Role { get; set; }

        [BsonElement("password")]
        public required string Password { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

    }
}