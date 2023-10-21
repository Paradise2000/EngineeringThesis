﻿using EngineeringThesisAPI.Entities;
using EngineeringThesisAPI.Services.UserIdProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace EngineeringThesisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly EngineeringThesisDbContext _context;
        private readonly IUserIdProvider _userIdProvider;

        public FileController(IWebHostEnvironment env, EngineeringThesisDbContext context, IUserIdProvider userIdProvider)
        {
            _env = env;
            _context = context;
            _userIdProvider = userIdProvider;
        }
        
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var supportedTypes = new[] { ".jpg", ".jpeg", ".png" };
            var fileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(fileName);

            if (!supportedTypes.Contains(fileExtension))
            {
                return BadRequest("File type is not supported.");
            }

            var filePath = new FilePath()
            {
                FileName = $"{Guid.NewGuid()}{fileExtension}",
                AddTime = DateTime.Now,
                UserId = _userIdProvider.GetUserId()
            };

            _context.FilePaths.Add(filePath);
            _context.SaveChanges();

            var path = Path.Combine(_env.ContentRootPath, "FileLocalStorage", filePath.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(filePath.FileName);
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(string id)
        {
            var path = Path.Combine(_env.ContentRootPath, "FileLocalStorage", id);
            var fileExtension = Path.GetExtension(path);

            if (System.IO.File.Exists(path))
            {
                byte[] b = await System.IO.File.ReadAllBytesAsync(path);

                if(fileExtension == ".jpg" || fileExtension == ".jpeg")
                {
                    return File(b, "image/jpeg");
                }

                if (fileExtension == ".png")
                {
                    return File(b, "image/png");
                }
                else
                {
                    return BadRequest("File extension problem");
                }
            }
            else
            {
                return BadRequest("File not found");
            }
        }      
    }
}
