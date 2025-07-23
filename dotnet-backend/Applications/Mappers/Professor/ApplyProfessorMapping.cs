using Applications.DTOs.Professor;
using Applications.DTOs.Student;
using AutoMapper;
using Domain.Entities;

namespace Applications.Mappers;

public partial class MappingProfile : Profile
{
    partial void ApplyProfessorMapping()
    {
        CreateMap<Professor, ProfessorResponse>()
            .ForMember(dest => dest.AcademicRank, 
                opt => opt.MapFrom(src => src.AcademicRank.ToString()))
            .ForMember(dest => dest.PersonFullName,
                opt => opt.MapFrom(src => src.Person != null
                    ? $"{src.Person.FirstName} {src.Person.LastName}"
                    : string.Empty));

        CreateMap<ProfessorRequest, Professor>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.AcademicRank, opt => opt.Condition(src => src.AcademicRank.HasValue))
            .ForMember(dest => dest.HireDate, opt => opt.Condition(src => src.HireDate.HasValue))
            .ForMember(dest => dest.Specialization, opt => opt.Condition(src => src.Specialization != null))
            .ForMember(dest => dest.OfficeLocation, opt => opt.Condition(src => src.OfficeLocation != null))
            .ForMember(dest => dest.Salary, opt => opt.Condition(src => src.Salary.HasValue))
            .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue))
            .ForMember(dest => dest.PersonId, opt => opt.Condition(src => src.PersonId != 0));
    }
}