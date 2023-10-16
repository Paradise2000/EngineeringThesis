using AutoMapper;
using EngineeringThesisAPI.DTOs.Attraction;
using EngineeringThesisAPI.Entities;
using EngineeringThesisAPI.Services.UserIdProvider;
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

        public AttractionController(EngineeringThesisDbContext context, IMapper mapper, IUserIdProvider userIdProvider)
        {
            _context = context;
            _mapper = mapper;
            _userIdProvider = userIdProvider;
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
    }
}
