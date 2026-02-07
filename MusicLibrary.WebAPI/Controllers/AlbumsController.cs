using Microsoft.AspNetCore.Mvc;
using MusicLibrary.Application.Abstractions.Services;
using MusicLibrary.Application.Requests.Album;
using MusicLibrary.Domain.Entities;
using MusicLibrary.WebAPI.Models;

namespace MusicLibrary.WebAPI.Controllers;

[ApiController]
[Route("api/artists/{artistId:guid}/albums")]
public class AlbumsController : ControllerBase
{
    private readonly IAlbumService _albumService;

    public AlbumsController(IAlbumService albumService)
    {  
        _albumService = albumService; 
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid artistId)
    {
        var albums = await _albumService.GetAlbumsByArtist(artistId);
        return Ok(albums);
    }

    [HttpGet("{albumId}")]
    public async Task<IActionResult> GetAlbumById(Guid artistId, Guid albumId)
    {
        var album = await _albumService.GetAlbumByIdAsync(artistId, albumId);
        return Ok(album);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Guid artistId, [FromBody] CreateAlbumBody body)
    {
        var request = new CreateAlbumRequest(artistId, body.Name);

        var album = await _albumService.CreateAlbumAsync(request);

        return CreatedAtAction(nameof(GetAlbumById), new { artistId, albumId = album.Id }, album);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid artistId,Guid albumId)
    {
        await _albumService.DeleteAlbumAsync(artistId, albumId);
        return NoContent();
    }   
}