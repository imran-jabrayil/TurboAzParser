namespace TurboAzParser.Clients.Settings;

public record TurboAzClientSettings
{
    public string BaseUrl { get; init; } = null!;
    public string PagePathTemplate { get; init; } = null!;
}