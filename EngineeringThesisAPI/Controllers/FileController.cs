using EngineeringThesisAPI.Entities;
using EngineeringThesisAPI.Services.UserIdProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace EngineeringThesisAPI.Controllers
{
    [Route("api/file")]
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
        [Authorize]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var supportedTypes = new[] { ".jpg", ".jpeg", ".png" };
            var fileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(fileName).ToLower();

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

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var path = Path.Combine(_env.ContentRootPath, "FileLocalStorage", id);

            var file = await _context.FilePaths.FirstOrDefaultAsync(x => x.FileName == id);

            if(await _context.Attractions.FirstOrDefaultAsync(r => r.Id == file.AttractionId && r.MainPhotoId == file.Id) != null)
            {
                return BadRequest("Cannot delete a photo assigned as main photo");
            }

            if(file != null)
            {
                _context.FilePaths.Remove(file);
                _context.SaveChanges();

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                return Ok("File deleted with path: " + path);
            }
            else
            {
                return BadRequest("File not found");
            }
        }

        [HttpGet("download/{id}")]
        [Authorize]
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
