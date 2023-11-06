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

        public AttractionController(EngineeringThesisDbContext context, IMapper mapper, IUserIdProvider userIdProvider, IValidator<AddCommentDto> AddCommentDto)
        {
            _context = context;
            _mapper = mapper;
            _userIdProvider = userIdProvider;
            _validatorAddCommentDto = AddCommentDto;
        }

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

        [HttpGet("get")]
        public ActionResult<PaginationModel<GetAttractionsDto>> GetAttractions(int pageIndex, int pageSize)
        {
            var attractions = _mapper.Map<List<GetAttractionsDto>>(
                _context.Attractions
                .Include(r => r.Category)
                .Include(r => r.Photos));

            var paginatedList = PaginationModel<GetAttractionsDto>.Create(attractions, pageIndex, pageSize);

            return Ok(paginatedList);
        }

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
    }
}
