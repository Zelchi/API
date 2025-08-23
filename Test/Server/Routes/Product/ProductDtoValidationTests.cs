using System.ComponentModel.DataAnnotations;
using Backend.Server.Routes.Product;
using FluentAssertions;

namespace Test.Server.Routes.Product;

[TestClass]
public class ProductDtoValidationTests
{
    private List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }

    [TestMethod]
    public void CreateProductDto_ValidData_ShouldPassValidation()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Produto Teste",
            Price = 100.50m,
            Description = "Descrição do produto teste"
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [TestMethod]
    public void CreateProductDto_EmptyName_ShouldFailValidation()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "",
            Price = 100.50m,
            Description = "Descrição do produto teste"
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(vr => vr.ErrorMessage.Contains("Nome é obrigatório"));
    }

    [TestMethod]
    public void CreateProductDto_ZeroPrice_ShouldFailValidation()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Produto Teste",
            Price = 0,
            Description = "Descrição do produto teste"
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(vr => vr.ErrorMessage.Contains("maior que zero"));
    }

    [TestMethod]
    public void CreateProductDto_NegativePrice_ShouldFailValidation()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Produto Teste",
            Price = -10.50m,
            Description = "Descrição do produto teste"
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(vr => vr.ErrorMessage.Contains("maior que zero"));
    }

    [TestMethod]
    public void CreateProductDto_MinimumValidPrice_ShouldPassValidation()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Produto Teste",
            Price = 0.01m,
            Description = "Descrição do produto teste"
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [TestMethod]
    public void UpdateProductDto_ValidData_ShouldPassValidation()
    {
        // Arrange
        var dto = new UpdateProductDto
        {
            Name = "Produto Atualizado",
            Price = 200.00m,
            Description = "Nova descrição"
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [TestMethod]
    public void UpdateProductDto_EmptyFields_ShouldPassValidation()
    {
        // Arrange
        var dto = new UpdateProductDto
        {
            Name = null,
            Price = null,
            Description = null
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        validationResults.Should().BeEmpty();
    }
}
