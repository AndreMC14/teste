using System.ComponentModel.DataAnnotations;

namespace MusicStreamingAPI.DTOs;

/// <summary>
/// DTO para login
/// </summary>
public class LoginDto
{
    [Required(ErrorMessage = "O e-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
    public string Senha { get; set; } = string.Empty;
}

/// <summary>
/// DTO para cadastro
/// </summary>
public class CadastroDto
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MaxLength(200)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "A confirmação de senha é obrigatória")]
    [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
    public string ConfirmarSenha { get; set; } = string.Empty;
}

/// <summary>
/// DTO para resposta de autenticação
/// </summary>
public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public UsuarioDto Usuario { get; set; } = null!;
}

/// <summary>
/// DTO para dados do usuário
/// </summary>
public class UsuarioDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ImagemPerfilUrl { get; set; }
    public int? PlanoId { get; set; }
}
