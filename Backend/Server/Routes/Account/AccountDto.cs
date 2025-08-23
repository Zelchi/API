using System.ComponentModel.DataAnnotations;

namespace Backend.Server.Routes.Account;

public record CreateAccountDto
{
    [Required(ErrorMessage = "Usuário é obrigatório")]
    [StringLength(50, ErrorMessage = "Usuário deve ter no máximo 50 caracteres")]
    public required string Username { get; init; }

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
    [StringLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(100, ErrorMessage = "Senha deve ter no máximo 100 caracteres")]
    public required string Password { get; init; }

    [Required(ErrorMessage = "Perfil é obrigatório")]
    [StringLength(20, ErrorMessage = "Perfil deve ter no máximo 20 caracteres")]
    public required string Role { get; init; }
}

public record UpdateAccountDto
{
    [StringLength(50, ErrorMessage = "Usuário deve ter no máximo 50 caracteres")]
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

public record LoginResponseDto
{
    public required string Token { get; init; }
    public required AccountEntity User { get; init; }
}