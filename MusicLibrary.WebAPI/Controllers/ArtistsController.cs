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
    private readonly IMusicService _musicService;

    public ArtistsController(IArtistService artistService,IMusicService musicService) 
    {  
        _artistService = artistService; 
        _musicService = musicService;
    }

    [HttpGet("{id:guid}")]
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

    [HttpPost("{artistId:guid}/albums/{albumId:guid}/musics/{musicId:guid}")]
    public async Task<IActionResult> AddMusicToAlbum(Guid artistId, Guid albumId, Guid musicId)
    {
        await _musicService.AddMusicToAlbumAsync(artistId, albumId, musicId);
        return NoContent();
    }

    [HttpDelete("{artistId:guid}/albums/{albumId:guid}/musics/{musicId:guid}")]
    public async Task<IActionResult> RemoveMusicFromAlbum(Guid artistId, Guid albumId, Guid musicId)
    {
        await _musicService.RemoveMusicFromAlbumAsync(artistId, albumId, musicId);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id) 
    {
        await _artistService.DeleteAsync(id);
        return NoContent();
    }
}