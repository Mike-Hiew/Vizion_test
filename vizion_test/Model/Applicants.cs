using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace vizion_test.Model;

public class Applicants
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { set; get; }

    [BsonElement("Name")]
    [Required]
    public string Name { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Email { get; set; } = null!;

    [MaxLength(20)]
    public string? Phone { get; set; } = null;

    public string? ApplyPosition { get; set; } = null!;

    public string? Profile { get; set; } = null!;
    public string? ResumeUrl { get; set; } = null!;

    [Required]
    public DateTime ApplyDate { get; set; }
    public string? Note { get; set; } = null!;
}
