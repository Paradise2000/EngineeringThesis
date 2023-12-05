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
        private readonly IWebHostEnvironment _env;

        public AttractionController(EngineeringThesisDbContext context, IMapper mapper, IUserIdProvider userIdProvider, IValidator<AddCommentDto> AddCommentDto, IValidator<UpdateCommentDto> validatorUpdateCommentDto, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _userIdProvider = userIdProvider;
            _validatorAddCommentDto = AddCommentDto;
            _validatorUpdateCommentDto = validatorUpdateCommentDto;
            _env = env;
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult CreateAttraction([FromBody]CreateAttractionDto dto)
        {
            bool isMainPhotoPicked = false;

            var attraction = _mapper.Map<Attraction>(dto);
            attraction.UserId = _userIdProvider.GetUserId();

            _context.Attractions.Add(attraction);

            foreach (var file in dto.ImagesPaths)
            {
                var record = _context.FilePaths.FirstOrDefault(r => r.FileName == file && r.UserId == _userIdProvider.GetUserId());

                if (record == null)
                {
                    return BadRequest($"Photo with name ${file} not found");
                }

                if(file == dto.MainImagePath)
                {
                    attraction.MainPhotoId = record.Id;
                    isMainPhotoPicked = true;
                }

                record.Attraction = attraction;
            }

            if(isMainPhotoPicked == false)
            {
                return BadRequest("Main photo not found");
            }

            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpPut("updateAttraction")]
        public IActionResult UpdateAttraction([FromBody]UpdateAttractionDto dto)
        {
            bool isMainPhotoPicked = false;

            var attraction = _context.Attractions.FirstOrDefault(r => r.Id == dto.AttractionId && r.UserId == _userIdProvider.GetUserId());   
            
            if(attraction == null)
            {
                return BadRequest("the attraction does not exist or does not belong to this user");
            }

            foreach(var file in dto.ImagesPaths) 
            {
                var record = _context.FilePaths.FirstOrDefault(r => r.FileName == file && r.UserId == _userIdProvider.GetUserId());

                if (record == null)
                {
                    return BadRequest("Photo not found");
                }

                if (file == dto.MainImagePath)
                {
                    attraction.MainPhotoId = record.Id;
                    isMainPhotoPicked = true;
                }

                record.AttractionId = attraction.Id;
            }

            if (isMainPhotoPicked == false)
            {
                return BadRequest("Main photo not found");
            }

            attraction.City = dto.City;
            attraction.Duration = dto.Duration;
            attraction.Price = dto.Price;
            attraction.CategoryId = dto.CategoryId;
            attraction.Name = dto.Name;
            attraction.Description = dto.Description;
            attraction.CoordinateX = dto.CoordinateX;
            attraction.CoordinateY = dto.CoordinateY;
            attraction.CoordinateZ = dto.CoordinateZ;

            var recordsToDelete = _context.FilePaths
                .Where(r => !dto.ImagesPaths.Contains(r.FileName) && r.AttractionId == attraction.Id)
                .ToList();

            _context.FilePaths.RemoveRange(recordsToDelete);

            _context.SaveChanges();

            foreach (var record in recordsToDelete)
            {
                var path = Path.Combine(_env.ContentRootPath, "FileLocalStorage", record.FileName);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

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

        [HttpGet("getUserAttractions")]
        [Authorize]
        public ActionResult<List<GetAttractionsDto>> GetUserAttractions()
        {
            var attractions =
                _context.Attractions
                .Include(r => r.Category)
                .Include(r => r.Photos)
                .Where(r => r.UserId == _userIdProvider.GetUserId());

            return Ok(_mapper.Map<List<GetAttractionsDto>>(attractions));
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
            result.isUserAttracion = false;

            if(_userIdProvider.IsUserLogged())
            {
                var userComment = _context.Comments.FirstOrDefault(r => r.UserId == _userIdProvider.GetUserId() && r.AttractionId == id);

                if (userComment != null)
                {
                    result.UserComment = _mapper.Map<GetCommentDto>(userComment);
                }

                if(attraction.UserId == _userIdProvider.GetUserId())
                {
                    result.isUserAttracion = true;
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

        [Authorize]
        [HttpPost("addAttractionToPlan/{AttractionId}")]
        public IActionResult AddAttractionToPlan(int AttractionId)
        {
            var attraction = _context.Attractions.FirstOrDefault(r => r.Id == AttractionId);

            if (attraction != null)
            {
                var user = _context.Users
                    .Include(r => r.TripAttractions)
                    .FirstOrDefault(r => r.Id == _userIdProvider.GetUserId());

                if(user.TripAttractions.FirstOrDefault(r => r.AttractionId == attraction.Id) != null)
                {
                    return BadRequest("Attraction already added");
                }

                user.TripAttractions.Add(new TripAttraction { UserId = user.Id, AttractionId = attraction.Id });

                _context.SaveChanges();

                return Ok();
            }
            else
            {
                return BadRequest("Attraction not found");
            }
        }

        [Authorize]
        [HttpDelete("deleteAttractionFromPlan/{AttractionId}")]
        public IActionResult DeleteAttractionFromPlan(int AttractionId)
        {
            var attraction = _context.TripAttractions.FirstOrDefault(r => r.UserId == _userIdProvider.GetUserId() && r.AttractionId == AttractionId);

            if (attraction == null)
            {
                return BadRequest("Attraction or user not found");
            }

            _context.TripAttractions.Remove(attraction);
            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpGet("getAttractionPlan")]
        public IActionResult GetAttractionPlan()
        {
            var attractions = _context.TripAttractions
                .Where(r => r.UserId == _userIdProvider.GetUserId())
                .Include(r => r.Attraction.Category)
                .Include(r => r.Attraction.Photos);

            var dto = new GetAttractionPlanDto
            {
                attractions = _mapper.Map<List<GetAttractionsDto>>(attractions.Select(r => r.Attraction)),
                locationSummary = _mapper.Map<List<LocationSummaryDto>>(attractions.Select(r => r.Attraction)),
                totalTime = attractions.Select(r => r.Attraction.Duration).AsEnumerable().Aggregate(TimeSpan.Zero, (currentSum, duration) => currentSum + duration),
                totalPrice = attractions.Select(r => r.Attraction.Price).Sum(),
            };

            return Ok(dto);
        }
    }
}
