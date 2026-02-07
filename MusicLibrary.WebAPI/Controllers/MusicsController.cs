using Microsoft.AspNetCore.Mvc;
using MusicLibrary.Application.Abstractions.Services;
using MusicLibrary.Application.Requests.Music;
using MusicLibrary.WebAPI.Models;

namespace MusicLibrary.WebAPI.Controllers;

[ApiController]
[Route("api/artists/{artistId:guid}/musics")]
public class MusicsController : ControllerBase
{
    IMusicService _musicService;

    public MusicsController(IMusicService musicService)
    {
        _musicService = musicService;
    }

    [HttpGet("{musicId:guid}")]
    public async Task<IActionResult> GetMusicsById(Guid artistId,Guid musicId)
    {
       var music = await _musicService.GetMusicByIdAsync(artistId, musicId);

        return Ok(music);
    }

    [HttpGet]
    public async Task<IActionResult> GetMusicsByArtist(Guid artistId)
    {
        var musics = await _musicService.GetMusicsByArtistAsync(artistId);
        return Ok(musics);
    }
     
    [HttpPost]
    public async Task<IActionResult> CreateMusic(Guid artistId, [FromBody] CreateMusicBody body)
    {
        var request = new CreateMusicRequest(artistId, body.Name, body.GetDuration());
        var music = await _musicService.CreateMusicAsync(request);
        return CreatedAtAction(nameof(GetMusicsById), new { artistId , musicId = music.Id }, music);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteMusic(Guid artistId, Guid musicId)
    {
        await _musicService.DeleteMusicAsync(artistId, musicId);
        return NoContent();
    }
}
