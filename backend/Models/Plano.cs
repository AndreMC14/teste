using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStreamingAPI.Models;

/// <summary>
/// Model: Plano
/// Representa um plano de assinatura
/// </summary>
public class Plano
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do plano é obrigatório")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descricao { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }

    /// <summary>
    /// Lista de benefícios do plano (armazenado como JSON)
    /// </summary>
    public string? BeneficiosJson { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Propriedade auxiliar para trabalhar com lista de benefícios
    /// </summary>
    [NotMapped]
    public List<string> Beneficios
    {
        get
        {
            if (string.IsNullOrEmpty(BeneficiosJson))
                return new List<string>();

            return System.Text.Json.JsonSerializer.Deserialize<List<string>>(BeneficiosJson) ?? new List<string>();
        }
        set
        {
            BeneficiosJson = System.Text.Json.JsonSerializer.Serialize(value);
        }
    }
}
