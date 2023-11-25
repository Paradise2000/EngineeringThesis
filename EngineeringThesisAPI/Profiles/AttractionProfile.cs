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
            CreateMap<AddCommentDto, Comment>()
                .ForMember(a => a.Date, b => b.MapFrom(c => DateTime.Now));
            CreateMap<UpdateCommentDto, Comment>();
            CreateMap<Comment, GetCommentDto>()
                .ForMember(a => a.Author, b => b.MapFrom(c => c.User.NickName));      
            CreateMap<Category, GetCategoriesDto>();
            CreateMap<Attraction, GetAttractionsDto>()
                .ForMember(a => a.CategoryName, b => b.MapFrom(c => c.Category.Name))
                .ForMember(a => a.MainImagePath, b => b.MapFrom(c => c.Photos.FirstOrDefault(r => r.Id == c.MainPhotoId).FileName));
            CreateMap<Attraction, GetAttractionDto>()
                .ForMember(a => a.CategoryName, b => b.MapFrom(c => c.Category != null ? c.Category.Name : null))
                .ForMember(a => a.AvgReview, b => b.MapFrom(c => c.Comments != null && c.Comments.Any() ? c.Comments.Average(r => r.Rating) : 0))
                .ForMember(a => a.NumberOfReviews, b => b.MapFrom(c => c.Comments != null ? c.Comments.Count() : 0))
                .ForMember(a => a.NumberOf5StarReviews, b => b.MapFrom(c => c.Comments != null ? c.Comments.Count(r => r.Rating == 5) : 0))
                .ForMember(a => a.NumberOf4StarReviews, b => b.MapFrom(c => c.Comments != null ? c.Comments.Count(r => r.Rating == 4) : 0))
                .ForMember(a => a.NumberOf3StarReviews, b => b.MapFrom(c => c.Comments != null ? c.Comments.Count(r => r.Rating == 3) : 0))
                .ForMember(a => a.NumberOf2StarReviews, b => b.MapFrom(c => c.Comments != null ? c.Comments.Count(r => r.Rating == 2) : 0))
                .ForMember(a => a.NumberOf1StarReviews, b => b.MapFrom(c => c.Comments != null ? c.Comments.Count(r => r.Rating == 1) : 0))
                .ForMember(a => a.ImagePaths, b => b.MapFrom(c => c.Photos.Select(r => r.FileName)))
                .ForMember(a => a.MainImagePath, b => b.MapFrom(c => c.Photos.FirstOrDefault(r => r.Id == c.MainPhotoId).FileName));
        }
    }
}
