using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStreamingAPI.Data;
using MusicStreamingAPI.Models;

namespace MusicStreamingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlanosController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PlanosController> _logger;

    public PlanosController(ApplicationDbContext context, ILogger<PlanosController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// GET: api/Planos
    /// Retorna todos os planos ativos
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<object>>> GetPlanos()
    {
        try
        {
            var planos = await _context.Planos
                .Where(p => p.Ativo)
                .ToListAsync();

            var planosDto = planos.Select(p => new
            {
                p.Id,
                p.Nome,
                p.Descricao,
                p.Preco,
                Beneficios = p.Beneficios
            }).ToList();

            return Ok(planosDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar planos");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// GET: api/Planos/5
    /// Retorna um plano específico por ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<object>> GetPlano(int id)
    {
        try
        {
            var plano = await _context.Planos.FindAsync(id);

            if (plano == null)
            {
                return NotFound(new { message = "Plano não encontrado" });
            }

            return Ok(new
            {
                plano.Id,
                plano.Nome,
                plano.Descricao,
                plano.Preco,
                Beneficios = plano.Beneficios
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar plano {Id}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }
}
