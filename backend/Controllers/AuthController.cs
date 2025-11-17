using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MusicStreamingAPI.DTOs;
using MusicStreamingAPI.Models;

namespace MusicStreamingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<Usuario> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<Usuario> userManager,
        IConfiguration configuration,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// POST: api/Auth/login
    /// Realiza login e retorna token JWT
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return Unauthorized(new { message = "E-mail ou senha inválidos" });
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Senha);

            if (!isPasswordValid)
            {
                return Unauthorized(new { message = "E-mail ou senha inválidos" });
            }

            var token = GenerateJwtToken(user);

            _logger.LogInformation("Usuário {Email} fez login com sucesso", user.Email);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Usuario = new UsuarioDto
                {
                    Id = user.Id,
                    Nome = user.Nome,
                    Email = user.Email!,
                    ImagemPerfilUrl = user.ImagemPerfilUrl,
                    PlanoId = user.PlanoId
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// POST: api/Auth/cadastro
    /// Cria novo usuário e retorna token JWT
    /// </summary>
    [HttpPost("cadastro")]
    public async Task<ActionResult<AuthResponseDto>> Cadastro([FromBody] CadastroDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar se e-mail já existe
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "E-mail já cadastrado" });
            }

            var user = new Usuario
            {
                UserName = dto.Email,
                Email = dto.Email,
                Nome = dto.Nome,
                DataCriacao = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Senha);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { message = "Erro ao criar usuário", errors });
            }

            var token = GenerateJwtToken(user);

            _logger.LogInformation("Novo usuário cadastrado: {Email}", user.Email);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Usuario = new UsuarioDto
                {
                    Id = user.Id,
                    Nome = user.Nome,
                    Email = user.Email!,
                    ImagemPerfilUrl = user.ImagemPerfilUrl,
                    PlanoId = user.PlanoId
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao cadastrar usuário");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Gera token JWT para o usuário
    /// </summary>
    private string GenerateJwtToken(Usuario user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Nome)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
