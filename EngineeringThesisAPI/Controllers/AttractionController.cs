using AutoMapper;
using EngineeringThesisAPI.DTOs.Attraction;
using EngineeringThesisAPI.Entities;
using EngineeringThesisAPI.Models.PaginationModel;
using EngineeringThesisAPI.Models.ValidationErrorModel;
using EngineeringThesisAPI.Services.UserIdProvider;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace EngineeringThesisAPI.Controllers
{
    [Route("api/attraction")]
    [ApiController]
    public class AttractionController : ControllerBase
    {
        private readonly EngineeringThesisDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IValidator<AddCommentDto> _validatorAddCommentDto;
        private readonly IValidator<UpdateCommentDto> _validatorUpdateCommentDto;

        public AttractionController(EngineeringThesisDbContext context, IMapper mapper, IUserIdProvider userIdProvider, IValidator<AddCommentDto> AddCommentDto, IValidator<UpdateCommentDto> validatorUpdateCommentDto)
        {
            _context = context;
            _mapper = mapper;
            _userIdProvider = userIdProvider;
            _validatorAddCommentDto = AddCommentDto;
            _validatorUpdateCommentDto = validatorUpdateCommentDto;
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult CreateAttraction([FromBody]CreateAttractionDto dto)
        {
            bool isMainPhotoPicked = false;

            var attraction = _mapper.Map<Attraction>(dto);

            attraction.UserId = _userIdProvider.GetUserId();

            _context.Attractions.Add(attraction);

            _context.SaveChanges(); //rozwiązanie tymczasowe

            foreach (var file in dto.ImagesPaths)
            {
                var record = _context.FilePaths.FirstOrDefault(r => r.FileName == file && r.UserId == _userIdProvider.GetUserId());

                if (record == null)
                {
                    _context.Remove(attraction); //rozwiązanie tymczasowe
                    return BadRequest("File name error");
                }

                if(file == dto.MainImagePath)
                {
                    attraction.MainPhotoId = record.Id;
                    isMainPhotoPicked = true;
                }

                record.AttractionId = attraction.Id;
            }

            if(isMainPhotoPicked == false)
            {
                _context.Remove(attraction); //rozwiązanie tymczasowe
                return BadRequest("File name error");
            }

            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("getCategories")]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<GetCategoriesDto>>(_context.Categories);

            return Ok(categories);
        }

        [HttpGet("getAttractions")]
        public ActionResult<PaginationModel<GetAttractionsDto>> GetAttractions(int pageIndex, int pageSize,
            string? City, int? CategoryId, string? Name)
        {
            var attractions =
                _context.Attractions
                .Include(r => r.Category)
                .Include(r => r.Photos)
                .AsQueryable();

            if(!string.IsNullOrEmpty(City)) 
            {
                attractions = attractions.Where(a => a.City.ToLower().Contains(City.ToLower()));
            }

            if(CategoryId.HasValue) 
            {
                attractions = attractions.Where(a => a.CategoryId == CategoryId);
            }

            if(!string.IsNullOrEmpty(Name))
            {
                attractions = attractions.Where(a => a.Name.ToLower().Contains(Name.ToLower()));
            }

            var paginatedList = PaginationModel<GetAttractionsDto>.Create(_mapper.Map<List<GetAttractionsDto>>(attractions), pageIndex, pageSize);

            return Ok(paginatedList);
        }

        [HttpGet("getAttraction")]
        public ActionResult<GetAttractionDto> GetAttraction(int id)
        {
            var attraction = _context.Attractions
                .Include(r => r.Category)
                .Include(r => r.Photos)
                .Include(r => r.Comments)
                .FirstOrDefault(r => r.Id == id);          

            var result = _mapper.Map<GetAttractionDto>(attraction);

            if(_userIdProvider.IsUserLogged())
            {
                var userComment = _context.Comments.FirstOrDefault(r => r.UserId == _userIdProvider.GetUserId() && r.AttractionId == id);

                if (userComment != null)
                {
                    result.UserComment = _mapper.Map<GetCommentDto>(userComment);
                }
            }

            return result;
        }

        [HttpGet("getComment")]
        public IActionResult GetComments(int attractionId, int pageIndex, int pageSize)
        {
            var comments = _mapper.Map<List<GetCommentDto>>(
                _context.Comments
                .Include(r => r.User)
                .Where(w => w.AttractionId == attractionId));

            var paginatedList = PaginationModel<GetCommentDto>.Create(comments, pageIndex, pageSize);

            return Ok(paginatedList);
        }

        [Authorize]
        [HttpPost("addComment")]
        public IActionResult AddComment([FromBody]AddCommentDto dto)
        {
            var commentValidation = _validatorAddCommentDto.Validate(dto);

            if(!commentValidation.IsValid)
            {
                return BadRequest(new ValidationErrorModel<AddCommentDto>(commentValidation));
            }

            var comment = _mapper.Map<Comment>(dto);
            comment.UserId = _userIdProvider.GetUserId();

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpPut("updateComment")]
        public IActionResult UpdateComment([FromBody]UpdateCommentDto dto)
        {
            var commentValidation = _validatorUpdateCommentDto.Validate(dto);

            if(!commentValidation.IsValid) 
            {
                return BadRequest(new ValidationErrorModel<UpdateCommentDto>(commentValidation));
            }

            var comment = _context.Comments
                .FirstOrDefault(r => r.UserId == _userIdProvider.GetUserId() && r.AttractionId == dto.AttractionId);

            if(comment != null)
            {
                comment.Title = dto.Title;
                comment.Rating = dto.Rating;
                comment.Description = dto.Description;
                _context.SaveChanges();
            } else
            {
                return BadRequest();
            }

            return Ok();
        }

        [Authorize]
        [HttpDelete("deleteComment/{AttractionId}")]
        public IActionResult DeleteComment(int AttractionId)
        {
            var comment = _context.Comments
                .FirstOrDefault(r => r.UserId == _userIdProvider.GetUserId() && r.AttractionId == AttractionId);

            if(comment == null)
            {
                return BadRequest("No comment to delete");
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return Ok();
        }
    }
}
