using System.ComponentModel.DataAnnotations;

namespace Backend.Server.Routes.Account;

public record CreateAccountDto
{
    [Required(ErrorMessage = "Usuário é obrigatório")]
    [StringLength(100, ErrorMessage = "Usuário deve ter no máximo 100 caracteres")]
    public required string Username { get; init; }

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
    [StringLength(255, ErrorMessage = "Email deve ter no máximo 255 caracteres")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(100, ErrorMessage = "Senha deve ter no máximo 100 caracteres")]
    public required string Password { get; init; }

    [Required(ErrorMessage = "Perfil é obrigatório")]
    [StringLength(30, ErrorMessage = "Perfil deve ter no máximo 30 caracteres")]
    public required string Role { get; init; }
}

public record UpdateAccountDto
{
    [StringLength(100, ErrorMessage = "Usuário deve ter no máximo 100 caracteres")]
    public string Username { get; init; }

    [StringLength(100, ErrorMessage = "Senha deve ter no máximo 100 caracteres")]
    public string Password { get; init; }

    [StringLength(30, ErrorMessage = "Função deve ter no máximo 30 caracteres")]
    public string Role { get; init; }
}

public record LoginDto
{
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "Senha é obrigatória")]
    public required string Password { get; init; }
}