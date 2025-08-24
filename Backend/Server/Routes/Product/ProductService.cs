using Backend.Server.Config;
using Microsoft.EntityFrameworkCore;

namespace Backend.Server.Routes.Product;

public class ProductService(ProductRepository productRepository)
{
    private readonly ProductRepository _productRepository = productRepository;

    public async Task<IEnumerable<ProductEntity>> GetAll(int accountId)
    {
        var products = await _productRepository.GetAllByAccountIdAsync(accountId);
        return products.Select(p => new ProductEntity
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            AccountId = p.AccountId,
            DeletedAt = p.DeletedAt,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        });
    }

    public async Task<ProductEntity> GetById(int id, int accountId)
    {
        var product = await _productRepository.GetByIdAsync(id, accountId);
        if (product == null)
            throw new Exception($"Produto com ID {id} não encontrado");
            
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

    public async Task<ProductEntity> Create(CreateProductDto createProductDto, int accountId)
    {
        ProductEntity product = new()
        {
            Name = createProductDto.Name,
            Price = createProductDto.Price,
            Description = createProductDto.Description,
            AccountId = accountId
        };

        var createdProduct = await _productRepository.CreateAsync(product);
        
        return new ProductEntity
        {
            Id = createdProduct.Id,
            Name = createdProduct.Name,
            Price = createdProduct.Price,
            Description = createdProduct.Description,
            AccountId = createdProduct.AccountId,
            DeletedAt = createdProduct.DeletedAt,
            CreatedAt = createdProduct.CreatedAt,
            UpdatedAt = createdProduct.UpdatedAt
        };
    }

    public async Task<ProductEntity> Update(int id, UpdateProductDto updateProductDto, int accountId)
    {
        var product = await _productRepository.GetByIdAsync(id, accountId);
        if (product == null)
            throw new Exception($"Produto com ID {id} não encontrado");

        if (!string.IsNullOrEmpty(updateProductDto.Name)) product.Name = updateProductDto.Name;
        if (updateProductDto.Price.HasValue && updateProductDto.Price > 0) product.Price = updateProductDto.Price.Value;
        if (!string.IsNullOrEmpty(updateProductDto.Description)) product.Description = updateProductDto.Description;

        var updatedProduct = await _productRepository.UpdateAsync(product);

        return new ProductEntity
        {
            Id = updatedProduct.Id,
            Name = updatedProduct.Name,
            Price = updatedProduct.Price,
            Description = updatedProduct.Description,
            AccountId = updatedProduct.AccountId,
            DeletedAt = updatedProduct.DeletedAt,
            CreatedAt = updatedProduct.CreatedAt,
            UpdatedAt = updatedProduct.UpdatedAt
        };
    }

    public async Task<bool> Delete(int id, int accountId)
    {
        var exists = await _productRepository.ExistsAsync(id, accountId);
        if (!exists) return false;

        await _productRepository.DeleteAsync(id, accountId);
        return true;
    }
}