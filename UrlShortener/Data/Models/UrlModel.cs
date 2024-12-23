using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Data.Models;

public class UrlModel
{
    [Key]
    public long Id { get; set; }
    [MaxLength(300)]
    public required string OriginalUrl { get; set; }
    [MaxLength(5)]
    public required string ShortUrlSuffix { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}