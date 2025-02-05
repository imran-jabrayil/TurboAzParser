using TurboAzParser.Models;

namespace TurboAzParser.Services.Abstractions;

public interface ITurboAzService
{
    Task<IEnumerable<string>> GetPageLinksAsync(uint pageId, uint carBrand);
    Task<CarInfo> GetCarInfoAsync(string url);
}