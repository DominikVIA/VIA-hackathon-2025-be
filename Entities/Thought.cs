using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Entities;

public class Thought
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } 
    
    public string Title { get; set; }

    public string Content { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTime.Now; 
}