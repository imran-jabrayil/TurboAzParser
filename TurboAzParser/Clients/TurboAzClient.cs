using AutoMapper;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using TurboAzParser.Clients.Abstractions;
using TurboAzParser.Clients.Settings;
using TurboAzParser.Models;

namespace TurboAzParser.Clients;

public class TurboAzClient(
    HttpClient httpClient,
    IOptions<TurboAzClientSettings> options,
    IMapper mapper) : ITurboAzClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly TurboAzClientSettings _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
    private readonly IMapper _mapper = mapper;


    public async Task<IEnumerable<string>> GetPageCarUrlsAsync(uint pageNumber, uint carBrandId)
    {
        string path = string.Format(
            _settings.PagePathTemplate,
            pageNumber,
            carBrandId);
        
        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        
        string response = await _httpClient.GetStringAsync(path);
        
        HtmlDocument doc = new();
        doc.LoadHtml(response);
        
        HtmlNodeCollection? cars = doc.DocumentNode
            .SelectNodes("//a[contains(@class, 'products-i__link')]");

        if (cars is null)
            return Array.Empty<string>();
        
        return cars.Where(car => car is not null)
            .Select(car => car.GetAttributeValue("href", "#"))
            .ToList();
    }
    
    public async Task<CarInfo> GetCarInfoAsync(string path)
    {
        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        string response = await _httpClient.GetStringAsync(path);
        
        HtmlDocument doc = new();
        doc.LoadHtml(response);
        
        HtmlNodeCollection? properties = doc.DocumentNode.SelectNodes("//div[contains(@class, 'product-properties__i')]");
        HtmlNodeCollection? statistics = doc.DocumentNode.SelectNodes("//span[contains(@class, 'product-statistics__i-text')]");

        HtmlNode? priceNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'product-price__i') and contains(@class, 'tz-mt-10')]")
                          ?? doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'product-price__i') and contains(@class, 'product-price__i--bold')]");

        string brand = this.getStringProperty(properties, "brand") ?? throw new NullReferenceException(nameof(brand));
        string model = this.getStringProperty(properties, "model") ?? throw new NullReferenceException(nameof(model));
        string priceRaw = priceNode?.InnerText ?? throw new ArgumentNullException(nameof(priceNode));

        var carInfoDto = new CarInfoDto
        {
            Brand = brand,
            Model = model,
            Price = priceRaw,
            LastUpdated = this.getStatsProperty(statistics, "lastUpdated") ?? DateTime.MinValue.ToString("dd.MM.yyyy"),
            ViewsCount = this.getStatsProperty(statistics, "viewsCount") ?? "-1",
            City = this.getStringProperty(properties, "city"),
            ReleaseDate = this.getStringProperty(properties, "releaseDate"),
            RoofType = this.getStringProperty(properties, "roofType"),
            Color = this.getStringProperty(properties, "color"),
            Engine = this.getStringProperty(properties, "engine"),
            Mileage = this.getStringProperty(properties, "mileage"),
            Gearbox = this.getStringProperty(properties, "gearbox"),
            Gear = this.getStringProperty(properties, "gear"),
            IsNew = this.getStringProperty(properties, "isNew"),
            SeatsCount = this.getStringProperty(properties, "seatsCount"),
            OwnersCount = this.getStringProperty(properties, "ownersCount"),
            State = this.getStringProperty(properties, "state"),
            Market = this.getStringProperty(properties, "market")
        };

        return _mapper.Map<CarInfo>(carInfoDto);
    }

    private string? getStringProperty(HtmlNodeCollection? properties, string field)
    {
        string labelText = field switch
        {
            "brand" => "Marka",
            "model" => "Model",
            "city" => "Şəhər",
            "releaseDate" => "Buraxılış ili",
            "roofType" => "Ban növü",
            "color" => "Rəng",
            "engine" => "Mühərrik",
            "mileage" => "Yürüş",
            "gearbox" => "Sürətlər qutusu",
            "gear" => "Ötürücü",
            "isNew" => "Yeni",
            "seatsCount" => "Yerlərin sayı",
            "ownersCount" => "Sahiblər",
            "state" => "Vəziyyəti",
            "market" => "Hansı bazar üçün yığılıb",
            _ => string.Empty
        };

        return properties?
            .FirstOrDefault(prop => prop.ChildNodes.Any(child => 
                child.Name == "label" && child.InnerText.Trim() == labelText))?
            .ChildNodes.FirstOrDefault(child => child.Name == "span")?
            .InnerText.Trim();
    }

    private string? getStatsProperty(HtmlNodeCollection? statistics, string field)
    {
        string startsWith = field switch
        {
            "lastUpdated" => "Yeniləndi: ",
            "viewsCount" => "Baxışların sayı: ",
            _ => string.Empty
        };

        return statistics?
            .FirstOrDefault(st => st.InnerText.Trim().StartsWith(startsWith))?
            .InnerText.Trim()
            .Substring(startsWith.Length);
    }
}