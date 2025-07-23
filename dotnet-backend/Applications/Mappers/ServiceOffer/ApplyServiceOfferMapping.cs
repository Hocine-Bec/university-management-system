using Applications.DTOs.ServiceOffer;
using AutoMapper;
using Domain.Entities;

namespace Applications.Mappers;

public partial class MappingProfile : Profile
{
    partial void ApplyServiceOfferMapping()
    {
        CreateMap<ServiceOffer, ServiceOfferResponse>().ReverseMap();
        
        CreateMap<ServiceOfferRequest, ServiceOffer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Fees, opt => opt.Condition(src => src.Fees.HasValue))
            .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
            .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue));
    }
}