using AutoMapper;
using EngineeringThesisAPI.DTOs.Attraction;
using EngineeringThesisAPI.Entities;
using EngineeringThesisAPI.Models.ValidationErrorModel;
using EngineeringThesisAPI.Services.UserIdProvider;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace EngineeringThesisAPI.Controllers
{
    [Route("api/attraction")]
    [ApiController]
    [Authorize]
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
            var attraction = _mapper.Map<Attraction>(dto);

            attraction.UserId = _userIdProvider.GetUserId();

            _context.Attractions.Add(attraction);
            _context.SaveChanges();

            return Ok();
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
