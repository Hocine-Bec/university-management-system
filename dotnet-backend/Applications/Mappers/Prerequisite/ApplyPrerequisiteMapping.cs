using Applications.DTOs.Prerequisite;
using AutoMapper;
using Domain.Entities;

namespace Applications.Mappers;

public partial class MappingProfile : Profile
{
    partial void ApplyPrerequisiteMapping()
    {
        CreateMap<PrerequisiteRequest, Prerequisite>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Course, opt => opt.Ignore())
            .ForMember(dest => dest.PrerequisiteCourse, opt => opt.Ignore())
            .ForMember(dest => dest.CourseId, opt => opt.Condition(src => src.CourseId.HasValue))
            .ForMember(dest => dest.PrerequisiteCourseId,
                opt => opt.Condition(src => src.PrerequisiteCourseId.HasValue));

        CreateMap<Prerequisite, PrerequisiteResponse>()
            .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Course.Code))
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
            .ForMember(dest => dest.PrerequisiteCourseCode, opt => opt.MapFrom(src => src.PrerequisiteCourse.Code))
            .ForMember(dest => dest.PrerequisiteCourseTitle, opt => opt.MapFrom(src => src.PrerequisiteCourse.Title));
    }
}