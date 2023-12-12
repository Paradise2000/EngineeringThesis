using AutoMapper;
using Azure.Core;
using EngineeringThesisAPI.DTOs.Account;
using EngineeringThesisAPI.DTOs.Attraction;
using EngineeringThesisAPI.Entities;
using EngineeringThesisAPI.Models.ValidationErrorModel;
using EngineeringThesisAPI.Services.UserIdProvider;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EngineeringThesisAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly EngineeringThesisDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IValidator<RegisterUserDto> _validatorRegisterUserDto;
        private readonly IValidator<ChangePasswordDto> _validatorChangePasswordDto;
        private readonly IMapper _mapper;
        private readonly IUserIdProvider _userIdProvider;

        public AccountController(EngineeringThesisDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IMapper mapper, IValidator<RegisterUserDto> validatorRegisterUserDto, IUserIdProvider userIdProvider, IValidator<ChangePasswordDto> validatorChangePasswordDto)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _mapper = mapper;
            _validatorRegisterUserDto = validatorRegisterUserDto;
            _userIdProvider = userIdProvider;
            _validatorChangePasswordDto = validatorChangePasswordDto;
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody]RegisterUserDto dto)
        {
            var registerValidation = _validatorRegisterUserDto.Validate(dto);

            if(!registerValidation.IsValid)
            {
                return BadRequest(new ValidationErrorModel(registerValidation));
            }

            var user = _mapper.Map<User>(dto);

            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.Password = hashedPassword;

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginUserDto dto)
        {
            var user = _context.Users.FirstOrDefault(e => e.Email == dto.Email);

            if (user == null)
            {
                return NotFound(); //user not found
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return BadRequest(); //wrong password
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return Ok(tokenHandler.WriteToken(token));

        }

        [HttpGet("getUserData")]
        [Authorize]
        public ActionResult<GetUserDataDto> GetUserData()
        {
            var user = _context.Users.FirstOrDefault(r => r.Id == _userIdProvider.GetUserId());

            if(user == null)
            {
                return NotFound("User not found");
            }

            var dto = _mapper.Map<GetUserDataDto>(user);

            return dto;
        }

        [HttpPut("setNewPassword")]
        [Authorize]
        public IActionResult SetNewPassword([FromBody]ChangePasswordDto dto)
        {
            var passwordValidation = _validatorChangePasswordDto.Validate(dto);

            if (!passwordValidation.IsValid)
            {
                return BadRequest(new ValidationErrorModel(passwordValidation));
            }

            var user = _context.Users.First(r => r.Id == _userIdProvider.GetUserId());

            user.Password = _passwordHasher.HashPassword(user, dto.NewPassword);

            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("isUserLogged")]
        [Authorize]
        public IActionResult IsUserLogged()
        {
            return Ok();
        }


    }
}
