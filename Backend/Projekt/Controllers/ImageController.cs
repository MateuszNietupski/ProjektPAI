using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Projekt.Data;
using Projekt.Models;
using Projekt.Models.DTOs;
using Projekt.Models.DTOs.Requests;

namespace Projekt.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ImageController(AppDbContext context, IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _hostEnvironment = hostEnvironment;
    }

    [Route("/api/imageAdd")]
    [HttpPost]
    public async Task<IActionResult> AddImage([FromForm] ImageDTO imageDto)
    {
        if (imageDto.File == null || imageDto.File.Length == 0)
        {
            return BadRequest("Nieprawidłowy plik obrazu.");
        }

        var FileName = $"{Guid.NewGuid().ToString()}_{imageDto.File.FileName}";
        var galleryFolder = Path.Combine(_hostEnvironment.WebRootPath, "GalleryImages");
        var filePath = Path.Combine(galleryFolder, FileName);

        using (var stream = new FileStream(filePath,FileMode.Create))
        {
            await imageDto.File.CopyToAsync(stream);
        }

        var id = Guid.NewGuid();
        var newImage = new Image
        {
            Id = id,
            fileName = FileName,
            filePath = $"/GalleryImages/{FileName}"
        };
        var max = _context.GalleryDisplaySequence
            .Select(g => g.Sequence)
            .ToList()
            .DefaultIfEmpty(0)
            .Max();
        max += 1;
        
        var gds = new GalleryDisplaySequence
        {
            Id = newImage.Id,
            Sequence = max,
        };
        _context.GalleryDisplaySequence.Add(gds);
        _context.Images.Add(newImage);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Route("/api/getGallery")]
    [HttpGet]
    [Authorize(Roles = "User,Admin")]
    public IActionResult GetGallery()
    {
        var sequence = _context.GalleryDisplaySequence.ToList<GalleryDisplaySequence>();
        var gallery = _context.Images
            .Join(_context.GalleryDisplaySequence,
                image => image.Id,
                gallery => gallery.Id,
                (image, gallery) => new { Image = image, Sequence = gallery.Sequence })
            .OrderBy(x => x.Sequence)
            .Select(x => x.Image)
            .ToList<Image>();
        return Ok(gallery);
    }

    [Route("/api/updateGallerySequence")]
    [HttpPatch]
    [Authorize(Roles = "User,Admin")]
    public IActionResult PatchGallerySequence([FromBody] List<UpdateGallerySequenceDTO> gallery)
    {
        try
        {
           // _context.GalleryDisplaySequence.RemoveRange(_context.GalleryDisplaySequence);
            
            foreach (var image in gallery)
            {
                var galleryImage = _context.GalleryDisplaySequence.FirstOrDefault(g => g.Id == image.id);
                if (galleryImage != null)
                {
                    galleryImage.Sequence = image.sequence;
                }
            }
            _context.SaveChanges();

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest($"Wystąpił błąd: {e.Message}");
        }
    }
}