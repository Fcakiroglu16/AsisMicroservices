using Microsoft.AspNetCore.Mvc;

namespace Microservice1.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    // Post : upload
    [HttpPost("upload")]
    public IActionResult UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("No file uploaded.");

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return Ok($"File uploaded successfully to: {filePath}");
    }
}