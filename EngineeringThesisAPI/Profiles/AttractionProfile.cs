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
            CreateMap<Category, GetCategoriesDto>();
            CreateMap<Attraction, GetAttractionsDto>()
                .ForMember(a => a.CategoryName, b => b.MapFrom(c => c.Category.Name))
                .ForMember(a => a.MainImagePath, b => b.MapFrom(c => c.Photos.FirstOrDefault(r => r.Id == c.MainPhotoId).FileName));
        }
    }
}
