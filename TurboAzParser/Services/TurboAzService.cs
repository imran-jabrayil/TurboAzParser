using TurboAzParser.Client.Abstractions;
using TurboAzParser.Models;
using TurboAzParser.Services.Abstractions;


namespace TurboAzParser.Services;

public class TurboAzService(
    ITurboAzClient client,
    ILogger<TurboAzService> logger)
    : ITurboAzService
{
    private readonly ITurboAzClient _client = client;
    private readonly ILogger<TurboAzService> _logger = logger;


    public async Task<IEnumerable<string>> GetPageLinksAsync(uint pageId, uint carBrand)
    {
        IEnumerable<string> urls = await _client.GetPageCarUrlsAsync(pageId, carBrand);

        urls = urls as List<string> ?? urls.ToList();
        
        foreach (string url in urls)
        {
            _logger.LogInformation($"Found car: {url}");
        }
        
        return urls;
    }

    public async Task<CarInfo> GetCarInfoAsync(string url)
    {
        CarInfo carInfo = await _client.GetCarInfoAsync(url);
        
        _logger.LogInformation($"Found car: {carInfo}");
        
        return carInfo;
    }
}