using Microsoft.AspNetCore.Mvc;
using MusicLibrary.Application.Abstractions.Services;
using MusicLibrary.Application.Requests.Artist;
using MusicLibrary.WebAPI.Models;

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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var artist = await _artistService.GetByIdAsync(id);
        return Ok(artist);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var artists = await _artistService.GetAllAsync();
        return Ok(artists);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArtistBody body)
    {
        var request = new CreateArtistRequest(body.Name);
        var artist = await _artistService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = artist.Id }, artist);
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