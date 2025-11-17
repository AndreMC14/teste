using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MusicStreamingAPI.Models;

/// <summary>
/// Model: Usuario
/// Representa um usuário do sistema (herda de IdentityUser para autenticação)
/// </summary>
public class Usuario : IdentityUser<int>
{
    [Required]
    [MaxLength(200)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ImagemPerfilUrl { get; set; }

    public int? PlanoId { get; set; }

    public Plano? Plano { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}
