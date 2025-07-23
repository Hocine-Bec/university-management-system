using Applications.DTOs.Grade;
using AutoMapper;
using Domain.Entities;

namespace Applications.Mappers;

public partial class MappingProfile : Profile
{
    partial void ApplyGradeMapping()
    {
        CreateMap<Grade, GradeResponse>()
            .ForMember(dest => dest.StudentNumber, opt => opt.MapFrom(src => src.Student.StudentNumber))
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
            .ForMember(dest => dest.SemesterTermCode, opt => opt.MapFrom(src => src.Semester.TermCode));

        CreateMap<GradeRequest, Grade>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Score, opt => opt.Condition(src => src.Score.HasValue))
            .ForMember(dest => dest.DateRecorded, opt => opt.Condition(src => src.DateRecorded.HasValue))
            .ForMember(dest => dest.Comments, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Comments)))
            .ForMember(dest => dest.StudentId, opt => opt.Condition(src => src.StudentId.HasValue))
            .ForMember(dest => dest.CourseId, opt => opt.Condition(src => src.CourseId.HasValue))
            .ForMember(dest => dest.SemesterId, opt => opt.Condition(src => src.SemesterId.HasValue))
            .ForMember(dest => dest.RegistrationId, opt => opt.Condition(src => src.RegistrationId.HasValue))
            .ForMember(dest => dest.Student, opt => opt.Ignore())
            .ForMember(dest => dest.Course, opt => opt.Ignore())
            .ForMember(dest => dest.Semester, opt => opt.Ignore())
            .ForMember(dest => dest.Registration, opt => opt.Ignore());
    }
}