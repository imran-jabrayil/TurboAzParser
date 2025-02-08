using System.Globalization;
using System.Text.RegularExpressions;
using AutoMapper;
using TurboAzParser.Models;

namespace TurboAzParser.Mappers;

public class CarInfoProfile : Profile
{
    public CarInfoProfile()
    {
        base.CreateMap<CarInfoDto, CarInfo>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
            .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src =>
                DateTime.ParseExact(src.LastUpdated, "dd.MM.yyyy", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.ViewsCount, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.ViewsCount) ? -1 : int.Parse(src.ViewsCount)))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City ?? string.Empty))
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
            .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.ReleaseDate) ? int.MinValue : int.Parse(src.ReleaseDate)))
            .ForMember(dest => dest.RoofType, opt => opt.MapFrom(src => src.RoofType ?? string.Empty))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color ?? string.Empty))
            .ForMember(dest => dest.Engine, opt => opt.MapFrom(src => src.Engine ?? string.Empty))
            .ForMember(dest => dest.Mileage, opt => opt.MapFrom(src => src.Mileage ?? string.Empty))
            .ForMember(dest => dest.Gearbox, opt => opt.MapFrom(src => src.Gearbox ?? string.Empty))
            .ForMember(dest => dest.Gear, opt => opt.MapFrom(src => src.Gear ?? string.Empty))
            .ForMember(dest => dest.IsNew, opt => opt.MapFrom(src => src.IsNew == "Bəli"))
            .ForMember(dest => dest.SeatsCount, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.SeatsCount) ? -1 : int.Parse(src.SeatsCount)))
            .ForMember(dest => dest.OwnersCount, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.OwnersCount) ? -1 : int.Parse(src.OwnersCount)))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State ?? string.Empty))
            .ForMember(dest => dest.Market, opt => opt.MapFrom(src => src.Market ?? string.Empty))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src =>
                int.Parse(Regex.Replace(src.Price, @"\D", ""))));
    }
}