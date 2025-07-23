using Applications.DTOs.Section;
using AutoMapper;
using Domain.Entities;

namespace Applications.Mappers;

public partial class MappingProfile : Profile
{
    partial void ApplySectionMapping()
    {
        CreateMap<SectionRequest, Section>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SectionNumber, opt => opt.Condition(src => !string.IsNullOrEmpty(src.SectionNumber)))
            .ForMember(dest => dest.MeetingDays, opt => opt.Condition(src => !string.IsNullOrEmpty(src.MeetingDays)))
            .ForMember(dest => dest.StartTime, opt => opt.Condition(src => src.StartTime.HasValue))
            .ForMember(dest => dest.EndTime, opt => opt.Condition(src => src.EndTime.HasValue))
            .ForMember(dest => dest.Classroom, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Classroom)))
            .ForMember(dest => dest.MaxCapacity, opt => opt.Condition(src => src.MaxCapacity.HasValue))
            .ForMember(dest => dest.CurrentEnrollment, opt => opt.Condition(src => src.CurrentEnrollment.HasValue))
            .ForMember(dest => dest.CourseId, opt => opt.Condition(src => src.CourseId.HasValue))
            .ForMember(dest => dest.SemesterId, opt => opt.Condition(src => src.SemesterId.HasValue))
            .ForMember(dest => dest.ProfessorId, opt => opt.Condition(src => src.ProfessorId.HasValue))
            .ForMember(dest => dest.Course, opt => opt.Ignore())
            .ForMember(dest => dest.Semester, opt => opt.Ignore())
            .ForMember(dest => dest.Professor, opt => opt.Ignore());

        CreateMap<Section, SectionResponse>()
            .ForMember(dest => dest.TimeSlot, opt => opt.MapFrom(src => $"{src.StartTime}--{src.EndTime}"));
    }
}