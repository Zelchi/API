using Microsoft.EntityFrameworkCore;
using Backend.Server.Config;

namespace Backend.Server.Routes.Product;

public class ProductRepository(Database Context)
{
    public async Task<IEnumerable<ProductEntity>> GetAllByAccountId(int accountId)
    {
        return await Context.Products.Where(p => p.AccountId == accountId && p.DeletedAt == DateTime.MinValue).ToListAsync();
    }

    public async Task<ProductEntity> GetById(int id, int accountId)
    {
        return await Context.Products.FirstOrDefaultAsync(p => p.Id == id && p.AccountId == accountId && p.DeletedAt == DateTime.MinValue);
    }

    public async Task<ProductEntity> Create(ProductEntity product)
    {
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;
        product.DeletedAt = DateTime.MinValue;

        Context.Products.Add(product);
        await Context.SaveChangesAsync();
        return product;
    }

    public async Task<ProductEntity> Update(ProductEntity product)
    {
        product.UpdatedAt = DateTime.UtcNow;
        Context.Products.Update(product);
        await Context.SaveChangesAsync();
        return product;
    }

    public async Task Delete(int id, int accountId)
    {
        var product = await GetById(id, accountId);
        if (product is not null)
        {
            product.DeletedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            await Update(product);
        }
    }

    public async Task<bool> Exists(int id, int accountId)
    {
        return await Context.Products.AnyAsync(p => p.Id == id && p.AccountId == accountId && p.DeletedAt == DateTime.MinValue);
    }
}
