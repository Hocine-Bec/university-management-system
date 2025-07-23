using Applications.DTOs.ServiceApplication;
using AutoMapper;
using Domain.Entities;

namespace Applications.Mappers;

public partial class MappingProfile : Profile
{
    partial void ApplyServiceApplicationMapping()
    {
        CreateMap<ServiceApplication, ServiceApplicationResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.PersonFullName, opt => opt.MapFrom(src
                => src.Person != null ? $"{src.Person.FirstName} {src.Person.LastName}" : string.Empty))
            .ForMember(dest => dest.ServiceOfferName, opt => opt.MapFrom(src
                => src.ServiceOffer != null ? src.ServiceOffer.Name : string.Empty));

        CreateMap<ServiceApplicationUpdateRequest, ServiceApplication>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ApplicationDate, opt => opt.Condition(src => src.ApplicationDate.HasValue))
            .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status.HasValue))
            .ForMember(dest => dest.PaidFees, opt => opt.Condition(src => src.PaidFees.HasValue))
            .ForMember(dest => dest.Notes, opt => opt.Condition(src => src.Notes != null))
            .ForMember(dest => dest.CompletedDate, opt => opt.Condition(src => src.CompletedDate.HasValue))
            .ForMember(dest => dest.PersonId, opt => opt.Condition(src => src.PersonId.HasValue))
            .ForMember(dest => dest.ServiceOfferId, opt => opt.Condition(src => src.ServiceOfferId.HasValue))
            .ForMember(dest => dest.ProcessedByUserId, opt => opt.Condition(src => src.ProcessedByUserId.HasValue))
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.ServiceOffer, opt => opt.Ignore())
            .ForMember(dest => dest.ProcessedByUser, opt => opt.Ignore());

        CreateMap<ServiceApplicationCreateRequest, ServiceApplication>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Person, opt => opt.Ignore())
            .ForMember(dest => dest.ServiceOffer, opt => opt.Ignore())
            .ForMember(dest => dest.ProcessedByUser, opt => opt.Ignore());
    }
}
