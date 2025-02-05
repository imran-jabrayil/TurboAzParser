using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using TurboAzParser.Client.Abstractions;
using TurboAzParser.Client.Settings;
using TurboAzParser.Models;

namespace TurboAzParser.Client;

public class TurboAzClient(
    HttpClient httpClient,
    IOptions<TurboAzClientSettings> options) : ITurboAzClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly TurboAzClientSettings _settings = options.Value ?? throw new ArgumentNullException(nameof(options));


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
        
        HtmlNodeCollection? properties = doc.DocumentNode
            .SelectNodes("//div[contains(@class, 'product-properties__i')]");
        HtmlNodeCollection? statistics = doc.DocumentNode
            .SelectNodes("//span[contains(@class, 'product-statistics__i-text')]");

        HtmlNode? priceNode = doc.DocumentNode
                                  .SelectSingleNode(
                                      "//div[contains(@class, 'product-price__i') and contains(@class, 'tz-mt-10')]")
                              ?? doc.DocumentNode
                                  .SelectSingleNode(
                                      "//div[contains(@class, 'product-price__i') and contains(@class, 'product-price__i--bold')]");


        string? brand = this.getStringProperty(properties, "brand");
        string? lastUpdated = this.getStatsProperty(statistics, "lastUpdated");
        string? viewsCount = this.getStatsProperty(statistics, "viewsCount");
        string? city = this.getStringProperty(properties, "city");
        string? model = this.getStringProperty(properties, "model");
        string? releaseDate = this.getStringProperty(properties, "releaseDate");
        string? roofType = this.getStringProperty(properties, "roofType");
        string? color = this.getStringProperty(properties, "color");
        string? engine = this.getStringProperty(properties, "engine");
        string? mileage = this.getStringProperty(properties, "mileage");
        string? gearbox = this.getStringProperty(properties, "gearbox");
        string? gear = this.getStringProperty(properties, "gear");
        string? isNew = this.getStringProperty(properties, "isNew");
        string? seatsCount = this.getStringProperty(properties, "seatsCount");
        string? ownersCount = this.getStringProperty(properties, "ownersCount");
        string? state = this.getStringProperty(properties, "state");
        string? market = this.getStringProperty(properties, "market");

        string price = Regex.Replace(priceNode?.InnerText ?? string.Empty, @"\D", "") ??
                       throw new ArgumentNullException(nameof(priceNode));
        
        var carInfo = new CarInfo
        {
            Brand = brand ?? throw new NullReferenceException("brand"),
            ViewsCount = viewsCount is not null 
                ? int.Parse(viewsCount) 
                : -1,
            LastUpdated = lastUpdated is not null
                ? DateTime.ParseExact(lastUpdated, "dd.MM.yyyy", CultureInfo.InvariantCulture)
                : DateTime.MinValue,
            City = city ?? string.Empty,
            Model = model ?? throw new NullReferenceException("model"),
            ReleaseDate = releaseDate is not null
                ? int.Parse(releaseDate)
                : int.MinValue,
            RoofType = roofType ?? string.Empty,
            Color = color ?? string.Empty,
            Engine = engine ?? string.Empty,
            Mileage = mileage ?? string.Empty,
            Gearbox = gearbox ?? string.Empty,
            Gear = gear ?? string.Empty,
            IsNew = isNew == "Bəli",
            SeatsCount = seatsCount is not null 
                ? int.Parse(seatsCount) 
                : -1,
            OwnersCount = ownersCount is not null 
                ? int.Parse(ownersCount) 
                : -1,
            State = state ?? string.Empty,
            Market = market ?? string.Empty,
            Price = int.Parse(price)
        };
        
        return carInfo;
    }

    private string? getStringProperty(HtmlNodeCollection properties, string field)
    {
        string labelText;
        
        switch (field)
        {
            case "brand":
                labelText = "Marka";
                break;
            case "city":
                labelText = "Şəhər";
                break;
            case "model":
                labelText = "Model";
                break;
            case "releaseDate":
                labelText = "Buraxılış ili";
                break;
            case "roofType":
                labelText = "Ban növü";
                break;
            case "color":
                labelText = "Rəng";
                break;
            case "engine":
                labelText = "Mühərrik";
                break;
            case "mileage":
                labelText = "Yürüş";
                break;
            case "gearbox":
                labelText = "Sürətlər qutusu";
                break;
            case "gear":
                labelText = "Ötürücü";
                break;
            case "isNew":
                labelText = "Yeni";
                break;
            case "seatsCount":
                labelText = "Yerlərin sayı";
                break;
            case "ownersCount":
                labelText = "Sahiblər";
                break;
            case "state":
                labelText = "Vəziyyəti";
                break;
            case "market":
                labelText = "Hansı bazar üçün yığılıb";
                break;
            
            default: return null;
        }

        return properties
            .FirstOrDefault(prop => prop.ChildNodes.Any(child => child.Name == "label" && child.InnerText == labelText))
            ?.ChildNodes.FirstOrDefault(child => child.Name == "span")?.InnerText;
    }

    private string? getStatsProperty(HtmlNodeCollection statistics, string field)
    {
        string startsWith;

        switch (field)
        {
            case "viewsCount":
                startsWith = "Baxışların sayı: ";
                break;
            case "lastUpdated":
                startsWith = "Yeniləndi: ";
                break;
            
            default: throw new InvalidOperationException();
        }

        return statistics.FirstOrDefault(st => st.InnerText.StartsWith(startsWith))
            ?.InnerText
            .Substring(startsWith.Length);
    }
}