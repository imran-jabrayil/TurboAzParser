namespace TurboAzParser.Models;

public record CarInfoDto
{
    public string Brand { get; set; } = null!;
    public string LastUpdated { get; set; } = null!;
    public string ViewsCount { get; set; } = null!;
    public string? City { get; set; }
    public string Model { get; set; } = null!;
    public string? ReleaseDate { get; set; }
    public string? RoofType { get; set; }
    public string? Color { get; set; }
    public string? Engine { get; set; }
    public string? Mileage { get; set; }
    public string? Gearbox { get; set; }
    public string? Gear { get; set; }
    public string? IsNew { get; set; }
    public string? SeatsCount { get; set; }
    public string? OwnersCount { get; set; }
    public string? State { get; set; }
    public string? Market { get; set; }
    public string Price { get; set; } = null!;
}