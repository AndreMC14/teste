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
    private readonly ILogger<MusicasController> _logger;

    public MusicasController(ApplicationDbContext context, ILogger<MusicasController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// GET: api/Musicas
    /// Retorna todas as músicas
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MusicaDto>>> GetMusicas()
    {
        try
        {
            var musicas = await _context.Musicas
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();

            var musicasDto = musicas.Select(m => MapToDto(m)).ToList();

            return Ok(musicasDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar músicas");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// GET: api/Musicas/5
    /// Retorna uma música específica por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<MusicaDto>> GetMusica(int id)
    {
        try
        {
            var musica = await _context.Musicas.FindAsync(id);

            if (musica == null)
            {
                return NotFound(new { message = "Música não encontrada" });
            }

            return Ok(MapToDto(musica));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar música {Id}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// GET: api/Musicas/buscar?termo=billie
    /// Busca músicas por termo (título, artista, álbum ou gênero)
    /// </summary>
    [HttpGet("buscar")]
    public async Task<ActionResult<IEnumerable<MusicaDto>>> BuscarMusicas([FromQuery] string termo)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(termo))
            {
                return BadRequest(new { message = "Termo de busca não pode ser vazio" });
            }

            var termoLower = termo.ToLower();

            var musicas = await _context.Musicas
                .Where(m =>
                    m.Titulo.ToLower().Contains(termoLower) ||
                    m.Artista.ToLower().Contains(termoLower) ||
                    (m.Album != null && m.Album.ToLower().Contains(termoLower)) ||
                    m.Genero.ToLower().Contains(termoLower))
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();

            var musicasDto = musicas.Select(m => MapToDto(m)).ToList();

            return Ok(musicasDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar músicas com termo {Termo}", termo);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// GET: api/Musicas/genero/Rock
    /// Busca músicas por gênero
    /// </summary>
    [HttpGet("genero/{genero}")]
    public async Task<ActionResult<IEnumerable<MusicaDto>>> GetMusicasPorGenero(string genero)
    {
        try
        {
            var musicas = await _context.Musicas
                .Where(m => m.Genero.ToLower() == genero.ToLower())
                .OrderByDescending(m => m.DataCriacao)
                .ToListAsync();

            var musicasDto = musicas.Select(m => MapToDto(m)).ToList();

            return Ok(musicasDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar músicas do gênero {Genero}", genero);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// POST: api/Musicas
    /// Cria uma nova música
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<MusicaDto>> CriarMusica([FromBody] CriarMusicaDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

            _logger.LogInformation("Música criada: {Titulo} - {Artista}", musica.Titulo, musica.Artista);

            return CreatedAtAction(nameof(GetMusica), new { id = musica.Id }, MapToDto(musica));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar música");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// PUT: api/Musicas/5
    /// Atualiza uma música existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarMusica(int id, [FromBody] AtualizarMusicaDto dto)
    {
        try
        {
            var musica = await _context.Musicas.FindAsync(id);

            if (musica == null)
            {
                return NotFound(new { message = "Música não encontrada" });
            }

            // Atualizar apenas campos não nulos
            if (!string.IsNullOrWhiteSpace(dto.Titulo))
                musica.Titulo = dto.Titulo;

            if (!string.IsNullOrWhiteSpace(dto.Artista))
                musica.Artista = dto.Artista;

            if (dto.Album != null)
                musica.Album = dto.Album;

            if (!string.IsNullOrWhiteSpace(dto.Genero))
                musica.Genero = dto.Genero;

            if (dto.Duracao.HasValue)
                musica.Duracao = dto.Duracao;

            if (dto.ImagemUrl != null)
                musica.ImagemUrl = dto.ImagemUrl;

            if (dto.AudioUrl != null)
                musica.AudioUrl = dto.AudioUrl;

            if (dto.AnoLancamento.HasValue)
                musica.AnoLancamento = dto.AnoLancamento;

            musica.DataAtualizacao = DateTime.UtcNow;

            _context.Entry(musica).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Música atualizada: {Id} - {Titulo}", musica.Id, musica.Titulo);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar música {Id}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// DELETE: api/Musicas/5
    /// Remove uma música
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarMusica(int id)
    {
        try
        {
            var musica = await _context.Musicas.FindAsync(id);

            if (musica == null)
            {
                return NotFound(new { message = "Música não encontrada" });
            }

            _context.Musicas.Remove(musica);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Música deletada: {Id} - {Titulo}", musica.Id, musica.Titulo);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar música {Id}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Mapeia Musica para MusicaDto
    /// </summary>
    private static MusicaDto MapToDto(Musica musica)
    {
        return new MusicaDto
        {
            Id = musica.Id,
            Titulo = musica.Titulo,
            Artista = musica.Artista,
            Album = musica.Album,
            Genero = musica.Genero,
            Duracao = musica.Duracao,
            ImagemUrl = musica.ImagemUrl,
            AudioUrl = musica.AudioUrl,
            AnoLancamento = musica.AnoLancamento,
            DataCriacao = musica.DataCriacao,
            DataAtualizacao = musica.DataAtualizacao
        };
    }
}
