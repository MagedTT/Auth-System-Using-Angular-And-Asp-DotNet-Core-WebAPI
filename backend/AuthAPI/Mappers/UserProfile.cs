using AuthAPI.DTOs;
using AuthAPI.Models;
using AutoMapper;

namespace AuthAPI.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterNewUserDto, User>()
            .ForMember(dest => dest.Id, options => options.Ignore());
    }
}