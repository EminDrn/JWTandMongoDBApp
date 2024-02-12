using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JWTApp.Models.Entities;

public class UserRefreshToken
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }


    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; }

    public string Code { get; set; }

    public DateTime Expiration { get; set; }
}