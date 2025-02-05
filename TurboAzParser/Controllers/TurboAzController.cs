using Microsoft.AspNetCore.Mvc;
using TurboAzParser.Models;
using TurboAzParser.Services.Abstractions;

namespace TurboAzParser.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TurboAzController(
    ILogger<TurboAzController> logger,
    ITurboAzService turboAzService) : Controller
{
    private readonly ILogger<TurboAzController> _logger = logger;
    private readonly ITurboAzService _turboAzService = turboAzService;


    [HttpGet]
    public async Task<IActionResult> GetTurboAzPageLinks([FromQuery] uint pageId, [FromQuery] uint carBrand)
    {
        IEnumerable<string> urls = await _turboAzService.GetPageLinksAsync(pageId, carBrand);
        
        foreach (string url in urls)
        {
            _logger.LogInformation($"Found car: {url}");
        }
        
        return base.Ok(urls);
    }

    [HttpGet]
    public async Task<IActionResult> GetTurboAzCarInfo([FromQuery] string url)
    {
        CarInfo carInfo = await _turboAzService.GetCarInfoAsync(url);
        
        return base.Ok(carInfo);
    }
}