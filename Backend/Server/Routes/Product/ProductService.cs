using Backend.Server.Config;
using Microsoft.EntityFrameworkCore;

namespace Backend.Server.Routes.Product;

public class ProductService(Database Context)
{
    public async Task<IEnumerable<ProductEntity>> GetAll(int accountId)
    {
        var products = await Context.Products
            .Where(p => p.DeletedAt == DateTime.MinValue && p.AccountId == accountId)
            .Select(p => new ProductEntity
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                AccountId = p.AccountId,
                DeletedAt = p.DeletedAt,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync();

        return products;
    }

    public async Task<ProductEntity> GetById(int id, int accountId)
    {
        var product = await Context.Products
            .Where(p => p.Id == id && p.DeletedAt == DateTime.MinValue && p.AccountId == accountId)
            .Select(p => new ProductEntity
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                AccountId = p.AccountId,
                DeletedAt = p.DeletedAt,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .FirstOrDefaultAsync() ?? throw new Exception($"Produto com ID {id} não encontrado");
            
        return product;
    }

    public async Task<ProductEntity> Create(CreateProductDto createProductDto, int accountId)
    {
        ProductEntity product = new()
        {
            Name = createProductDto.Name,
            Price = createProductDto.Price,
            Description = createProductDto.Description,
            AccountId = accountId,
            DeletedAt = DateTime.MinValue,
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
            AccountId = product.AccountId,
            DeletedAt = product.DeletedAt,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    public async Task<ProductEntity> Update(int id, UpdateProductDto updateProductDto, int accountId)
    {
        var product = await Context.Products.FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == DateTime.MinValue && p.AccountId == accountId) ??
            throw new Exception($"Produto com ID {id} não encontrado");

        if (!string.IsNullOrEmpty(updateProductDto.Name)) product.Name = updateProductDto.Name;

        if (updateProductDto.Price.HasValue && updateProductDto.Price > 0) product.Price = updateProductDto.Price.Value;
        
        if (!string.IsNullOrEmpty(updateProductDto.Description)) product.Description = updateProductDto.Description;

        product.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync();

        return new ProductEntity
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            AccountId = product.AccountId,
            DeletedAt = product.DeletedAt,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    public async Task<bool> Delete(int id, int accountId)
    {
        var product = await Context.Products.FirstOrDefaultAsync(p => p.Id == id && p.DeletedAt == DateTime.MinValue && p.AccountId == accountId);

        if (product == null) return false;

        product.DeletedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync();

        return true;
    }
}