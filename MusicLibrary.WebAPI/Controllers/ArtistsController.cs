using Microsoft.AspNetCore.Mvc;
using MusicLibrary.Application.Abstractions.Services;
using MusicLibrary.Application.Requests.Artist;

namespace MusicLibrary.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistsController : ControllerBase
{
    private readonly IArtistService _artistService;
    
    public ArtistsController(IArtistService artistService) 
    {  
        _artistService = artistService; 
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArtistRequest request)
    {
        var artist = await _artistService.CreateAsync(request);

        return CreatedAtAction(
        nameof(GetById),
        new { id = artist.Id },
        artist
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var artists = await _artistService.GetAllAsync();
        
        return Ok(artists);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id) 
    {
        var artist = await _artistService.GetByIdAsync(id);

        return Ok(artist);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id,[FromBody] UpdateArtistRequest request)
    {
        var artist = await _artistService.UpdateAsync(id,request);

        return Ok(artist);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id) 
    {
        await _artistService.DeleteAsync(id);

        return NoContent();
    }
}
