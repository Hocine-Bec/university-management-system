using Applications.DTOs.DocsVerification;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Mappers;

public partial class MappingProfile : Profile
{
    partial void ApplyDocsVerificationMapping()
    {
        CreateMap<DocsVerification, DocsVerificationResponse>()
            .ForMember(dest => dest.PersonFullName,
                opt => opt.MapFrom(src => src.Person != null
                    ? $"{src.Person.FirstName} {src.Person.LastName}"
                    : string.Empty))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<DocsVerificationRequest, DocsVerification>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SubmissionDate, opt => opt.Condition(src => src.SubmissionDate.HasValue))
            .ForMember(dest => dest.VerificationDate, opt => opt.Condition(src => src.VerificationDate.HasValue))
            .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status.HasValue))
            .ForMember(dest => dest.IsApproved, opt => opt.Condition(src => src.IsApproved.HasValue))
            .ForMember(dest => dest.RejectedReason, opt => opt.Condition(src => !string.IsNullOrEmpty(src.RejectedReason)))
            .ForMember(dest => dest.PaidFees, opt => opt.Condition(src => src.PaidFees.HasValue))
            .ForMember(dest => dest.Notes, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Notes)))
            .ForMember(dest => dest.VerifiedByUserId, opt => opt.Condition(src => src.VerifiedByUserId.HasValue))
            .ForMember(dest => dest.VerifiedByUser, opt => opt.Ignore())
            .ForMember(dest => dest.PersonId, opt => opt.Condition(src => src.PersonId.HasValue))
            .ForMember(dest => dest.Person, opt => opt.Ignore());
    }
}