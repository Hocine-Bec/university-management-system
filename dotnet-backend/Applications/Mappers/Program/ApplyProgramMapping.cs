using Applications.DTOs.Program;
using AutoMapper;
using Domain.Entities;

namespace Applications.Mappers;

public partial class MappingProfile : Profile
{
    partial void ApplyProgramMapping()
    {
        CreateMap<Program, ProgramResponse>().ReverseMap();
        
        CreateMap<ProgramRequest, Program>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Code, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Code)))
            .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
            .ForMember(dest => dest.MinimumAge, opt => opt.Condition(src => src.MinimumAge.HasValue))
            .ForMember(dest => dest.Duration, opt => opt.Condition(src => src.Duration.HasValue))
            .ForMember(dest => dest.Fees, opt => opt.Condition(src => src.Fees.HasValue))
            .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue));
    }
}