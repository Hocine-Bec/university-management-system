using Applications.DTOs.Registration;
using AutoMapper;
using Domain.Entities;

namespace Applications.Mappers;

public partial class MappingProfile : Profile
{
    partial void ApplyRegistrationMapping()
    {
        CreateMap<RegistrationRequest, Registration>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrationDate, opt => opt.Condition(src => src.RegistrationDate.HasValue))
            .ForMember(dest => dest.RegistrationFees, opt => opt.Condition(src => src.RegistrationFees.HasValue))
            .ForMember(dest => dest.Student, opt => opt.Ignore())
            .ForMember(dest => dest.Section, opt => opt.Ignore())
            .ForMember(dest => dest.Semester, opt => opt.Ignore())
            .ForMember(dest => dest.ProcessedByUser, opt => opt.Ignore());

        CreateMap<Registration, RegistrationResponse>()
            .ForMember(dest => dest.StudentNumber, opt => opt.MapFrom(src => src.Student.StudentNumber))
            .ForMember(dest => dest.SectionNumber, opt => opt.MapFrom(src => src.Section.SectionNumber))
            .ForMember(dest => dest.SemesterTermCode, opt => opt.MapFrom(src => src.Semester.TermCode));
            
    }
}