using System.ComponentModel.DataAnnotations;

namespace Backend.Server.Routes.Product;

public record CreateProductDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Preço é obrigatório")]
    [Range(0.01, 999999.99, ErrorMessage = "Preço deve ser maior que zero")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
    public string Description { get; set; }
}

public record UpdateProductDto
{
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Name { get; set; }

    [Range(0.01, 999999.99, ErrorMessage = "Preço deve ser maior que zero")]
    public decimal? Price { get; set; }
    
    [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
    public string Description { get; set; }
}