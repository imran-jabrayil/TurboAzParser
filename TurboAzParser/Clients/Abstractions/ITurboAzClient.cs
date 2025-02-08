using TurboAzParser.Models;

namespace TurboAzParser.Clients.Abstractions;

public interface ITurboAzClient
{
    Task<IEnumerable<string>> GetPageCarUrlsAsync(uint pageNumber, uint carBrandId);
    Task<CarInfo> GetCarInfoAsync(string path);
}