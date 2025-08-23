using Microsoft.AspNetCore.Mvc;

namespace Backend.Server.Routes.Product;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController(ProductService productService) : ControllerBase
{
    private readonly ProductService ProductService = productService;

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await ProductService.GetAll();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await ProductService.GetById(id);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = await ProductService.Create(createProductDto);
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = await ProductService.Update(id, updateProductDto);
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await ProductService.Delete(id);
        
        if (!deleted)
            return NotFound(new { message = "Produto não encontrado" });

        return Ok(new { message = "Produto removido com sucesso" });
    }
}