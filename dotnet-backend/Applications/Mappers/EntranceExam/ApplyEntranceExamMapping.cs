using Applications.DTOs.EntranceExam;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Mappers;

public partial class MappingProfile : Profile
{
    partial void ApplyEntranceExamMapping()
    {
        CreateMap<EntranceExam, EntranceExamResponse>()
            .ForMember(dest => dest.ExamStatus, opt => opt.MapFrom(src => src.ExamStatus.ToString()));

        CreateMap<EntranceExamRequest, EntranceExam>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ExamDate, opt => opt.Condition(src => src.ExamDate.HasValue))
            .ForMember(dest => dest.Score, opt => opt.Condition(src => src.Score.HasValue))
            .ForMember(dest => dest.IsPassed, opt => opt.Condition(src => src.IsPassed.HasValue))
            .ForMember(dest => dest.PaidFees, opt => opt.Condition(src => src.PaidFees.HasValue))
            .ForMember(dest => dest.ExamStatus, opt => opt.Condition(src => src.ExamStatus.HasValue))
            .ForMember(dest => dest.Notes, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Notes)));
    }
}