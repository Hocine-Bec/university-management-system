using Applications.DTOs;
using AutoMapper;
using Domain.Entities;
namespace Applications.Mappers
{
    public partial class MappingProfile : Profile
    {
        partial void ApplyCountryMapping()
        {
            CreateMap<Country, CountryResponse>().ReverseMap();

            CreateMap<CountryResponse, Country>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CountryResponse, Country>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
