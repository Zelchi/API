using System.ComponentModel.DataAnnotations;
using Backend.Server.Routes.Account;
using FluentAssertions;

namespace Test.Server.Routes.Account;

[TestClass]
public class AccountDtoValidationTests
{
    private List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }

    [TestMethod]
    public void CreateAccountDto_ValidData_ShouldPassValidation()
    {
        // Arrange
        var dto = new CreateAccountDto
        {
            Username = "testuser",
            Email = "test@test.com",
            Password = "password123",
            Role = "User"
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [TestMethod]
    public void CreateAccountDto_EmptyUsername_ShouldFailValidation()
    {
        // Arrange
        var dto = new CreateAccountDto
        {
            Username = "",
            Email = "test@test.com",
            Password = "password123",
            Role = "User"
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(vr => vr.ErrorMessage.Contains("Usuário é obrigatório"));
    }

    [TestMethod]
    public void CreateAccountDto_InvalidEmail_ShouldFailValidation()
    {
        // Arrange
        var dto = new CreateAccountDto
        {
            Username = "testuser",
            Email = "invalid-email",
            Password = "password123",
            Role = "User"
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(vr => vr.ErrorMessage.Contains("formato válido"));
    }

    [TestMethod]
    public void CreateAccountDto_EmptyPassword_ShouldFailValidation()
    {
        // Arrange
        var dto = new CreateAccountDto
        {
            Username = "testuser",
            Email = "test@test.com",
            Password = "",
            Role = "User"
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(vr => vr.ErrorMessage.Contains("Senha é obrigatória"));
    }

    [TestMethod]
    public void LoginDto_ValidData_ShouldPassValidation()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "test@test.com",
            Password = "password123"
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [TestMethod]
    public void LoginDto_InvalidEmail_ShouldFailValidation()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "invalid-email",
            Password = "password123"
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(vr => vr.ErrorMessage.Contains("formato válido"));
    }

    [TestMethod]
    public void UpdateAccountDto_ValidData_ShouldPassValidation()
    {
        // Arrange
        var dto = new UpdateAccountDto
        {
            Username = "updateduser",
            Password = "newpassword",
            Role = "Admin"
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        validationResults.Should().BeEmpty();
    }
}
