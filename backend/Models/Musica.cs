using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStreamingAPI.Models;

/// <summary>
/// Model: Musica
/// Representa uma música no sistema
/// </summary>
public class Musica
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O título é obrigatório")]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O artista é obrigatório")]
    [MaxLength(200)]
    public string Artista { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Album { get; set; }

    [Required(ErrorMessage = "O gênero é obrigatório")]
    [MaxLength(50)]
    public string Genero { get; set; } = string.Empty;

    /// <summary>
    /// Duração em segundos
    /// </summary>
    public int? Duracao { get; set; }

    [MaxLength(500)]
    public string? ImagemUrl { get; set; }

    [MaxLength(500)]
    public string? AudioUrl { get; set; }

    public int? AnoLancamento { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
}
