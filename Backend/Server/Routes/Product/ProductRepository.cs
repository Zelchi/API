using Backend.Server.Config;
using Microsoft.EntityFrameworkCore;

namespace Backend.Server.Routes.Product;

public class ProductRepository(Database Context)
{
    public async Task<IEnumerable<ProductEntity>> GetAllByAccountIdAsync(int accountId)
    {
        return await Context.Products
            .Where(p => p.AccountId == accountId && p.DeletedAt == DateTime.MinValue)
            .ToListAsync();
    }

    public async Task<ProductEntity> GetByIdAsync(int id, int accountId)
    {
        return await Context.Products
            .FirstOrDefaultAsync(p => p.Id == id && p.AccountId == accountId && p.DeletedAt == DateTime.MinValue);
    }

    public async Task<ProductEntity> CreateAsync(ProductEntity product)
    {
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;
        product.DeletedAt = DateTime.MinValue;

        Context.Products.Add(product);
        await Context.SaveChangesAsync();
        return product;
    }

    public async Task<ProductEntity> UpdateAsync(ProductEntity product)
    {
        product.UpdatedAt = DateTime.UtcNow;
        Context.Products.Update(product);
        await Context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteAsync(int id, int accountId)
    {
        var product = await GetByIdAsync(id, accountId);
        if (product != null)
        {
            product.DeletedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            await UpdateAsync(product);
        }
    }

    public async Task<bool> ExistsAsync(int id, int accountId)
    {
        return await Context.Products
            .AnyAsync(p => p.Id == id && p.AccountId == accountId && p.DeletedAt == DateTime.MinValue);
    }
}
