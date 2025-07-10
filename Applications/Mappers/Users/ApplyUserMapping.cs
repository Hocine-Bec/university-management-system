using Applications.DTOs.Users;
using AutoMapper;
using Domain.Entities;

namespace Applications.Mappers
{
    public partial class MappingProfile : Profile
    {
        partial void ApplyUserMapping()
        {
            CreateMap<User, UserResponse>();

            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.Person, opt => opt.Ignore());

            CreateMap<UpdateUserRequest, User>()
                .ForMember(dest => dest.Person, opt => opt.Ignore())
                .ForMember(dest => dest.PersonId, opt => opt.Condition(src => src.PersonId.HasValue))
                .ForMember(dest => dest.Username, opt => opt.Condition(src => src.Username != null)) 
                .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue));
        }
    }
}