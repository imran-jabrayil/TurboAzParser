using TurboAzParser.Models;

namespace TurboAzParser.Client.Abstractions;

public interface ITurboAzClient
{
    Task<IEnumerable<string>> GetPageCarUrlsAsync(uint pageNumber, uint carBrandId);
    Task<CarInfo> GetCarInfoAsync(string path);
}