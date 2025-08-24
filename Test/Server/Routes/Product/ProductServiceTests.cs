using Backend.Server.Routes.Product;
using Backend.Server.Routes.Account;
using FluentAssertions;
using Test.Server.Config;

namespace Test.Server.Routes.Product;

[TestClass]
public class ProductServiceTests : TestBase
{
    private ProductService _productService;
    private int _testAccountId = 1;

    [TestInitialize]
    public override void Setup()
    {
        base.Setup();
        var productRepository = new ProductRepository(_context);
        _productService = new ProductService(productRepository);
    }

    private void SeedTestDataForProducts()
    {
        var testAccount = new AccountEntity
        {
            Username = "testuser",
            Email = "test@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("password123"),
            Role = "User",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Accounts.Add(testAccount);
        _context.SaveChanges();
        _testAccountId = testAccount.Id;

        var testProduct = new ProductEntity
        {
            Name = "Produto Teste",
            Price = 100.50m,
            Description = "Descrição do produto teste",
            AccountId = _testAccountId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Products.Add(testProduct);
        _context.SaveChanges();
    }

    [TestMethod]
    public async Task Create_ValidProduct_ShouldReturnCreatedProduct()
    {
        // Arrange
        SeedTestDataForProducts();
        var createDto = new CreateProductDto
        {
            Name = "Novo Produto",
            Price = 250.75m,
            Description = "Descrição do novo produto"
        };

        // Act
        var result = await _productService.Create(createDto, _testAccountId);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Novo Produto");
        result.Price.Should().Be(250.75m);
        result.Description.Should().Be("Descrição do novo produto");
        result.AccountId.Should().Be(_testAccountId);
        result.Id.Should().BeGreaterThan(0);

        var savedProduct = await _context.Products.FindAsync(result.Id);
        savedProduct.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetAll_ShouldReturnUserProducts()
    {
        // Arrange
        SeedTestDataForProducts();
        
        // Act
        var result = await _productService.GetAll(_testAccountId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(0);
        result.All(p => p.AccountId == _testAccountId).Should().BeTrue();
        result.All(p => p.DeletedAt == DateTime.MinValue).Should().BeTrue();
    }

    [TestMethod]
    public async Task GetById_ExistingProduct_ShouldReturnProduct()
    {
        // Arrange
        SeedTestDataForProducts();
        var existingProduct = _context.Products.First(p => p.AccountId == _testAccountId);

        // Act
        var result = await _productService.GetById(existingProduct.Id, _testAccountId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(existingProduct.Id);
        result.Name.Should().Be(existingProduct.Name);
        result.AccountId.Should().Be(_testAccountId);
    }

    [TestMethod]
    public async Task GetById_NonExistingProduct_ShouldThrowException()
    {
        // Arrange
        var nonExistingId = 999;

        // Act & Assert
        var action = async () => await _productService.GetById(nonExistingId, _testAccountId);
        await action.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task GetById_ProductFromDifferentUser_ShouldThrowException()
    {
        // Arrange
        SeedTestDataForProducts();
        var otherAccount = new AccountEntity
        {
            Username = "otheruser",
            Email = "other@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("password123"),
            Role = "User"
        };
        _context.Accounts.Add(otherAccount);
        _context.SaveChanges();

        var existingProduct = _context.Products.First(p => p.AccountId == _testAccountId);

        // Act & Assert
        var action = async () => await _productService.GetById(existingProduct.Id, otherAccount.Id);
        await action.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task Update_ValidData_ShouldUpdateProduct()
    {
        // Arrange
        SeedTestDataForProducts();
        var existingProduct = _context.Products.First(p => p.AccountId == _testAccountId);
        var updateDto = new UpdateProductDto
        {
            Name = "Produto Atualizado",
            Price = 300.00m,
            Description = "Nova descrição"
        };

        // Act
        var result = await _productService.Update(existingProduct.Id, updateDto, _testAccountId);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Produto Atualizado");
        result.Price.Should().Be(300.00m);
        result.Description.Should().Be("Nova descrição");
        result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
    }

    [TestMethod]
    public async Task Update_NonExistingProduct_ShouldThrowException()
    {
        // Arrange
        var updateDto = new UpdateProductDto
        {
            Name = "Produto Atualizado"
        };

        // Act & Assert
        var action = async () => await _productService.Update(999, updateDto, _testAccountId);
        await action.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task Delete_ExistingProduct_ShouldReturnTrue()
    {
        // Arrange
        SeedTestDataForProducts();
        var existingProduct = _context.Products.First(p => p.AccountId == _testAccountId);

        // Act
        var result = await _productService.Delete(existingProduct.Id, _testAccountId);

        // Assert
        result.Should().BeTrue();

        _context.Entry(existingProduct).Reload();
        existingProduct.DeletedAt.Should().NotBe(DateTime.MinValue);
    }

    [TestMethod]
    public async Task Delete_NonExistingProduct_ShouldReturnFalse()
    {
        // Act
        var result = await _productService.Delete(999, _testAccountId);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task Delete_ProductFromDifferentUser_ShouldReturnFalse()
    {
        // Arrange
        SeedTestDataForProducts();
        var otherAccount = new AccountEntity
        {
            Username = "otheruser",
            Email = "other@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("password123"),
            Role = "User"
        };
        _context.Accounts.Add(otherAccount);
        _context.SaveChanges();

        var existingProduct = _context.Products.First(p => p.AccountId == _testAccountId);

        // Act
        var result = await _productService.Delete(existingProduct.Id, otherAccount.Id);

        // Assert
        result.Should().BeFalse();
    }
}
