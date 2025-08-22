using Backend.Server.Config;
using Microsoft.EntityFrameworkCore;

namespace Backend.Server.Routes.Product;

public class ProductService(Database context)
{
    private readonly Database Context = context;

    public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
    {
        var products = await Context.Products
            .Where(p => p.Active)
            .Select(p => new ProductEntity
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Active = p.Active,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync();

        return products;
    }

    public async Task<ProductEntity> GetProductByIdAsync(int id)
    {
        var product = await Context.Products
            .Where(p => p.Id == id && p.Active)
            .Select(p => new ProductEntity
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Active = p.Active,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"Produto com ID {id} não encontrado");
        return product;
    }

    public async Task<ProductEntity> CreateProductAsync(CreateProductDto createProductDto)
    {
        ProductEntity product = new()
        {
            Name = createProductDto.Name,
            Price = createProductDto.Price,
            Description = createProductDto.Description,
            Active = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Products.Add(product);
        await Context.SaveChangesAsync();

        return new ProductEntity
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            Active = product.Active,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    public async Task<ProductEntity> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
    {
        var product = await Context.Products
            .FirstOrDefaultAsync(p => p.Id == id && p.Active);

        if (product == null)
            throw new KeyNotFoundException($"Produto com ID {id} não encontrado");

        if (!string.IsNullOrEmpty(updateProductDto.Name))
            product.Name = updateProductDto.Name;
        
        if (updateProductDto.Price.HasValue)
            product.Price = updateProductDto.Price.Value;
        
        if (!string.IsNullOrEmpty(updateProductDto.Description))
            product.Description = updateProductDto.Description;
        
        if (updateProductDto.Active.HasValue)
            product.Active = updateProductDto.Active.Value;

        product.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync();

        return new ProductEntity
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            Active = product.Active,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await Context.Products
            .FirstOrDefaultAsync(p => p.Id == id && p.Active);

        if (product == null)
            return false;

        product.Active = false;
        product.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync();
        return true;
    }
}