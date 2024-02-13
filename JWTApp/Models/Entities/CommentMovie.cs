using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JWTApp.Models.Entities;

public class CommentMovie
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string MovieId { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; }
    public string Comment { get; set; }
    public DateTime CreateTime { get; set; }
}