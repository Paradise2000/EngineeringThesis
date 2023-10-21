using AutoMapper;
using EngineeringThesisAPI.DTOs.Attraction;
using EngineeringThesisAPI.Entities;
using EngineeringThesisAPI.Services.UserIdProvider;

namespace EngineeringThesisAPI.Profiles
{
    public class AttractionProfile : Profile
    {
        public AttractionProfile()
        {
            CreateMap<CreateAttractionDto, Attraction>();
            CreateMap<AddCommentDto, Comment>();
        }
    }
}
