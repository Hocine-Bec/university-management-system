using Applications.DTOs.FinancialHold;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Mappers;

public partial class MappingProfile : Profile
{
    partial void ApplyFinancialHoldMapping()
    {
        CreateMap<FinancialHoldRequest, FinancialHold>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Reason, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Reason)))
            .ForMember(dest => dest.HoldAmount, opt => opt.Condition(src => src.HoldAmount.HasValue))
            .ForMember(dest => dest.DatePlaced, opt => opt.Condition(src => src.DatePlaced.HasValue))
            .ForMember(dest => dest.DateResolved, opt => opt.Condition(src => src.DateResolved.HasValue))
            .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue))
            .ForMember(dest => dest.ResolutionNotes, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ResolutionNotes)))
            .ForMember(dest => dest.StudentId, opt => opt.Condition(src => src.StudentId.HasValue))
            .ForMember(dest => dest.PlacedByUserId, opt => opt.Condition(src => src.PlacedByUserId.HasValue))
            .ForMember(dest => dest.ResolvedByUserId, opt => opt.Condition(src => src.ResolvedByUserId.HasValue))
            .ForMember(dest => dest.Student, opt => opt.Ignore())
            .ForMember(dest => dest.PlacedByUser, opt => opt.Ignore())
            .ForMember(dest => dest.ResolvedByUser, opt => opt.Ignore());

        CreateMap<FinancialHold, FinancialHoldResponse>();
    }
}