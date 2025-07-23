using Applications.DTOs.Student;
using AutoMapper;
using Domain.Entities;

namespace Applications.Mappers
{
    public partial class MappingProfile : Profile
    {
        partial void ApplyStudentMapping()
        {
            CreateMap<Student, StudentResponse>()
           .ForMember(dest => dest.Status, 
                opt => opt.MapFrom(src => src.StudentStatus.ToString()))
           .ForMember(dest => dest.PersonFullName,
                opt => opt.MapFrom(src => src.Person != null
                   ? $"{src.Person.FirstName} {src.Person.LastName}"
                   : string.Empty));

            CreateMap<StudentRequest, Student>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StudentNumber, opt => opt.Condition(src => src.StudentNumber != null))
                .ForMember(dest => dest.StudentStatus, opt => opt.Condition(src => src.StudentStatus.HasValue))
                .ForMember(dest => dest.Notes, opt => opt.Condition(src => src.Notes != null))
                .ForMember(dest => dest.PersonId, opt => opt.Condition(src => src.PersonId != 0));
        }
    }
}
