using Applications.DTOs.People;
using AutoMapper;
using Domain.Entities;

namespace Applications.Mappers
{
    public partial class MappingProfile : Profile
    {
        partial void ApplyPersonMapping()
        {
            CreateMap<Person, PersonResponse>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.CountryName,
                    opt => opt.MapFrom(src => src.Country.Name));

            CreateMap<PersonRequest, Person>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FirstName, opt => opt.Condition(src => src.FirstName != null))
                .ForMember(dest => dest.LastName, opt => opt.Condition(src => src.LastName != null))
                .ForMember(dest => dest.DOB, opt => opt.Condition(src => src.DOB.HasValue))
                .ForMember(dest => dest.Address, opt => opt.Condition(src => src.Address != null))
                .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => src.PhoneNumber != null))
                .ForMember(dest => dest.Email, opt => opt.Condition(src => src.Email != null))
                .ForMember(dest => dest.ImagePath, opt => opt.Condition(src => src.ImagePath != null))
                .ForMember(dest => dest.CountryId, opt => opt.Condition(src => src.CountryId.HasValue));
        }
    }
}