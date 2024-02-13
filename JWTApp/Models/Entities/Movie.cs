using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JWTApp.Models.Entities;

public class Movie
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string MovieName { get; set; }

    public string MovieDescription { get; set; }

    public double Rating { get; set; }

    public DateTime ReleaseDate { get; set; }

    public string MoviePhoto { get; set; }
    
    public double RateCounter { get; set; }

}