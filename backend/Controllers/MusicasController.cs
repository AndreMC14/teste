using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStreamingAPI.Data;
using MusicStreamingAPI.DTOs;
using MusicStreamingAPI.Models;

namespace MusicStreamingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MusicasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MusicasController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Musica>>> GetMusicas()
    {
        return await _context.Musicas.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Musica>> GetMusica(int id)
    {
        var musica = await _context.Musicas.FindAsync(id);

        if (musica == null)
        {
            return NotFound();
        }

        return musica;
    }

    [HttpGet("buscar")]
    public async Task<ActionResult<IEnumerable<Musica>>> BuscarMusicas([FromQuery] string termo)
    {
        return await _context.Musicas
            .Where(m => m.Titulo.Contains(termo) || 
                       m.Artista.Contains(termo) || 
                       m.Genero.Contains(termo))
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Musica>> CriarMusica(CriarMusicaDto dto)
    {
        var musica = new Musica
        {
            Titulo = dto.Titulo,
            Artista = dto.Artista,
            Album = dto.Album,
            Genero = dto.Genero,
            Duracao = dto.Duracao,
            ImagemUrl = dto.ImagemUrl,
            AudioUrl = dto.AudioUrl,
            AnoLancamento = dto.AnoLancamento,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };

        _context.Musicas.Add(musica);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMusica), new { id = musica.Id }, musica);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarMusica(int id, AtualizarMusicaDto dto)
    {
        var musica = await _context.Musicas.FindAsync(id);

        if (musica == null)
        {
            return NotFound();
        }

        if (dto.Titulo != null) musica.Titulo = dto.Titulo;
        if (dto.Artista != null) musica.Artista = dto.Artista;
        if (dto.Album != null) musica.Album = dto.Album;
        if (dto.Genero != null) musica.Genero = dto.Genero;
        if (dto.Duracao.HasValue) musica.Duracao = dto.Duracao;
        if (dto.ImagemUrl != null) musica.ImagemUrl = dto.ImagemUrl;
        if (dto.AudioUrl != null) musica.AudioUrl = dto.AudioUrl;
        if (dto.AnoLancamento.HasValue) musica.AnoLancamento = dto.AnoLancamento;
        
        musica.DataAtualizacao = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarMusica(int id)
    {
        var musica = await _context.Musicas.FindAsync(id);

        if (musica == null)
        {
            return NotFound();
        }

        _context.Musicas.Remove(musica);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
