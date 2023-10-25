using AutoMapper;
using EngineeringThesisAPI.DTOs.Account;
using EngineeringThesisAPI.Entities;

namespace EngineeringThesisAPI.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile() 
        {
            CreateMap<RegisterUserDto, User>()
                .ForMember(a => a.NickName, b => b.MapFrom(c => c.NickName))
                .ForMember(a => a.Email, b => b.MapFrom(c => c.Email))
                .ForMember(a => a.Password, b => b.MapFrom(c => c.Password))
                .ForMember(a => a.Enabled, b => b.MapFrom(c => true));
        }
    }
}
