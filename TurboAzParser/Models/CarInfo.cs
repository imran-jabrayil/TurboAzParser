namespace TurboAzParser.Models;

public record CarInfo
{
    public required DateTime LastUpdated { get; set; }
    public required int ViewsCount { get; set; }
    public required string City { get; init; }
    public required string Brand { get; init; }
    public required string Model { get; init; }
    public required int ReleaseDate { get; init; }
    public required string RoofType { get; init; }
    public required string Color { get; init; } 
    public required string Engine { get; init; }
    public required string Mileage { get; init; }
    public required string Gearbox { get; init; }
    public required string Gear { get; init; }
    public required bool IsNew { get; init; }
    public required int SeatsCount { get; init; }
    public required int OwnersCount { get; init; }
    public required string State { get; init; }
    public required string Market { get; init; }
    
    public required int Price { get; init; }
}