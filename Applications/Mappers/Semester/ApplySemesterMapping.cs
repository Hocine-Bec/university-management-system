using Applications.DTOs.Semester;
using AutoMapper;
using Domain.Entities;

namespace Applications.Mappers;

public partial class MappingProfile : Profile
{
    partial void ApplySemesterMapping()
    {
        CreateMap<SemesterRequest, Semester>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.Term, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Term)))
            .ForMember(dest => dest.Year, opt => opt.Condition(src => src.Year.HasValue))
            .ForMember(dest => dest.StartDate, opt => opt.Condition(src => src.StartDate.HasValue))
            .ForMember(dest => dest.EndDate, opt => opt.Condition(src => src.EndDate.HasValue))
            .ForMember(dest => dest.RegStartsAt, opt => opt.Condition(src => src.RegStartsAt.HasValue))
            .ForMember(dest => dest.RegEndsAt, opt => opt.Condition(src => src.RegEndsAt.HasValue))
            .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue));

        CreateMap<Semester, SemesterResponse>().ReverseMap();
    }
}