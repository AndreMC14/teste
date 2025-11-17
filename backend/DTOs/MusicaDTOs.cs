using System.ComponentModel.DataAnnotations;

namespace MusicStreamingAPI.DTOs;

/// <summary>
/// DTO para criação de música
/// </summary>
public class CriarMusicaDto
{
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

    public int? Duracao { get; set; }

    [MaxLength(500)]
    public string? ImagemUrl { get; set; }

    [MaxLength(500)]
    public string? AudioUrl { get; set; }

    public int? AnoLancamento { get; set; }
}

/// <summary>
/// DTO para atualização de música
/// </summary>
public class AtualizarMusicaDto
{
    [MaxLength(200)]
    public string? Titulo { get; set; }

    [MaxLength(200)]
    public string? Artista { get; set; }

    [MaxLength(200)]
    public string? Album { get; set; }

    [MaxLength(50)]
    public string? Genero { get; set; }

    public int? Duracao { get; set; }

    [MaxLength(500)]
    public string? ImagemUrl { get; set; }

    [MaxLength(500)]
    public string? AudioUrl { get; set; }

    public int? AnoLancamento { get; set; }
}

/// <summary>
/// DTO para resposta de música
/// </summary>
public class MusicaDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Artista { get; set; } = string.Empty;
    public string? Album { get; set; }
    public string Genero { get; set; } = string.Empty;
    public int? Duracao { get; set; }
    public string? ImagemUrl { get; set; }
    public string? AudioUrl { get; set; }
    public int? AnoLancamento { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao { get; set; }
}
