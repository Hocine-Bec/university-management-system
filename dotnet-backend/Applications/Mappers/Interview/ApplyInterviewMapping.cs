using Applications.DTOs.Interview;
using AutoMapper;
using Domain.Entities;

namespace Applications.Mappers;

public partial class MappingProfile : Profile
{
    partial void ApplyInterviewMapping()
    {
        CreateMap<Interview, InterviewResponse>()
            .ForMember(dest => dest.ProfessorFullName, opt => opt.MapFrom(src => 
                src.Professor != null ? $"{src.Professor.Person.FirstName} {src.Professor.Person.LastName}" : null));
        
        CreateMap<InterviewRequest, Interview>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ScheduledDate, opt => opt.Condition(src => src.ScheduledDate.HasValue))
            .ForMember(dest => dest.StartTime, opt => opt.Condition(src => src.StartTime.HasValue))
            .ForMember(dest => dest.EndTime, opt => opt.Condition(src => src.EndTime.HasValue))
            .ForMember(dest => dest.IsApproved, opt => opt.Condition(src => src.IsApproved.HasValue))
            .ForMember(dest => dest.PaidFees, opt => opt.Condition(src => src.PaidFees.HasValue))
            .ForMember(dest => dest.Notes, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Notes)))
            .ForMember(dest => dest.Recommendation, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Recommendation)))
            .ForMember(dest => dest.ProfessorId, opt => opt.Condition(src => src.ProfessorId.HasValue))
            .ForMember(dest => dest.Professor, opt => opt.Ignore());
    }
}