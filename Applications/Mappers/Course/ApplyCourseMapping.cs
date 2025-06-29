using Applications.DTOs.Course;
using AutoMapper;
using Domain.Entities;

namespace Applications.Mappers;

public partial class MappingProfile : Profile
{
    partial void ApplyCourseMapping()
    {
        CreateMap<CourseRequest, Course>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Code, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Code)))
            .ForMember(dest => dest.Title, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Title)))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
            .ForMember(dest => dest.CreditHours, opt => opt.Condition(src => src.CreditHours.HasValue))
            .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue));

        CreateMap<Course, CourseResponse>().ReverseMap();
    }
}
    