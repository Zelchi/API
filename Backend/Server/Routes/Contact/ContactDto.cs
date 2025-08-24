using System.ComponentModel.DataAnnotations;

namespace Backend.Server.Routes.Contact;

public record CreateContactDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Telefone é obrigatório")]
    [StringLength(15, ErrorMessage = "Telefone deve ter no máximo 15 caracteres")]
    public string Phone { get; set; }
    
    [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
    [StringLength(255, ErrorMessage = "Email deve ter no máximo 255 caracteres")]
    public string Email { get; set; }
}

public record UpdateContactDto
{
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Name { get; set; }
    
    [StringLength(15, ErrorMessage = "Telefone deve ter no máximo 15 caracteres")]
    public string Phone { get; set; }
    
    [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
    [StringLength(255, ErrorMessage = "Email deve ter no máximo 255 caracteres")]
    public string Email { get; set; }
}