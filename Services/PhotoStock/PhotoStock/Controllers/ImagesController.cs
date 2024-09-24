using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PhotoStock.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly string _uploadFolderPath;

    public ImagesController()
    {
        _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");
    }

    //TODO: FolderName artir, sekildeki bosluqlari sil.
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile image, CancellationToken cancellationToken)
    {
        Guard.Against.Null(image, nameof(image));
        Guard.Against.LengthOutOfRange(nameof(image), 1, int.MaxValue);

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(image.FileName);

        if(string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
        {
            return BadRequest("Invalid file extension");
        }

        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
        var path = Path.Combine(_uploadFolderPath, fileName);

        try
        {
            Directory.CreateDirectory(_uploadFolderPath);
            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        string imageUrl = $"{Request.Scheme}://{Request.Host}/ProductImages/{fileName}";

        return Ok(new { Url = imageUrl});
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string imageUrl)
    {
        Guard.Against.NullOrEmpty(imageUrl, nameof(imageUrl));

        var fileName = Path.GetFileName(imageUrl);
        var path = Path.Combine(_uploadFolderPath, fileName);

        if (System.IO.File.Exists(path))
        {
            try
            {
                System.IO.File.Delete(path);
                return Ok(new { message = "Image deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while deleting the image: {ex.Message}");
            }
        }

        return NotFound(new { message = "Image not found" });
    }
}
