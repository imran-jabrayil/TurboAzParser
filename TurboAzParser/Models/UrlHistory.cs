using System.ComponentModel.DataAnnotations;
using TurboAzParser.Models.Enums;

namespace TurboAzParser.Models;

public class UrlHistory
{
    public long Id { get; set; }
    [MaxLength(50)]
    public string Url { get; set; } = null!;
    public ParsingStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

