using MongoDB.Bson.Serialization.Attributes;

namespace EVConnectService.Models
{
    public class User
    {
        [BsonId]  // This replaces MongoDB's _id with NIC
        [BsonElement("_id")]
        public string NIC { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

         [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

    }
}