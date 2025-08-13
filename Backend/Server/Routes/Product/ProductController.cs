using Microsoft.AspNetCore.Mvc;

namespace Backend.Server.Routes.Product;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllProducts()
    {
        var products = new[]
        {
            new { Id = 1, Name = "Notebook", Price = 2500.00 },
            new { Id = 2, Name = "Mouse", Price = 50.00 },
            new { Id = 3, Name = "Teclado", Price = 150.00 }
        };

        return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var product = new { Id = id, Name = $"Produto {id}", Price = 100.00 * id };
        return Ok(product);
    }

    [HttpPost]
    public IActionResult CreateProduct([FromBody] object product)
    {
        return Ok(new { message = "Produto criado com sucesso!", product });
    }
}
