using Microsoft.AspNetCore.Mvc;
using Backend.Server.Routes.Account;
using FluentAssertions;
using Test.Server.Config;

namespace Test.Server.Routes.Account;

[TestClass]
public class AccountControllerTests : TestBase
{
    private AccountService AccountService;
    private AccountController Controller;

    [TestInitialize]
    public override void Setup()
    {
        base.Setup();
        var accountRepository = new AccountRepository(Context);
        AccountService = new AccountService(accountRepository, Configuration);
        Controller = new AccountController(AccountService);
    }

    [TestMethod]
    public async Task CreateAccount_ValidData_ShouldReturnCreated()
    {
        // Arrange
        var createDto = new CreateAccountDto
        {
            Username = "testuser",
            Email = "test@test.com",
            Password = "password123",
            Role = "User"
        };

        // Act
        var result = await Controller.CreateAccount(createDto);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = (CreatedAtActionResult)result;
        var createdAccount = createdResult.Value as AccountEntity;
        
        createdAccount.Should().NotBeNull();
        createdAccount.Username.Should().Be("testuser");
        createdAccount.Email.Should().Be("test@test.com");
    }

    [TestMethod]
    public async Task CreateAccount_DuplicateEmail_ShouldReturnBadRequest()
    {
        // Arrange
        SeedTestData();
        var createDto = new CreateAccountDto
        {
            Username = "newuser",
            Email = "test@test.com", 
            Password = "password123",
            Role = "User"
        };

        // Act
        var result = await Controller.CreateAccount(createDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [TestMethod]
    public async Task Login_ValidCredentials_ShouldReturnOk()
    {
        // Arrange
        SeedTestData();
        var loginDto = new LoginDto
        {
            Email = "test@test.com",
            Password = "password123"
        };

        // Act
        var result = await Controller.Login(loginDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        var loginResponse = okResult.Value as LoginResponseDto;
        
        loginResponse.Should().NotBeNull();
        loginResponse.Token.Should().NotBeNullOrEmpty();
        loginResponse.Username.Should().NotBeNull();
    }

    [TestMethod]
    public async Task Login_InvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "invalid@test.com",
            Password = "wrongpassword"
        };

        // Act
        var result = await Controller.Login(loginDto);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [TestMethod]
    public async Task GetAccountById_ExistingId_ShouldReturnOk()
    {
        // Arrange
        SeedTestData();
        var existingAccount = Context.Accounts.First();

        // Act
        var result = await Controller.GetAccountById(existingAccount.Id);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        var account = okResult.Value as AccountEntity;
        
        account.Should().NotBeNull();
        account.Id.Should().Be(existingAccount.Id);
    }

    [TestMethod]
    public async Task GetAccountById_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        var result = await Controller.GetAccountById(999);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [TestMethod]
    public async Task UpdateAccount_ValidData_ShouldReturnOk()
    {
        // Arrange
        SeedTestData();
        var existingAccount = Context.Accounts.First();
        var updateDto = new UpdateAccountDto
        {
            Username = "updateduser",
            Role = "Admin"
        };

        // Act
        var result = await Controller.UpdateAccount(existingAccount.Id, updateDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        var updatedAccount = okResult.Value as AccountEntity;
        
        updatedAccount.Should().NotBeNull();
        updatedAccount.Username.Should().Be("updateduser");
        updatedAccount.Role.Should().Be("Admin");
    }

    [TestMethod]
    public async Task DeleteAccount_ExistingId_ShouldReturnOk()
    {
        // Arrange
        SeedTestData();
        var existingAccount = Context.Accounts.First();

        // Act
        var result = await Controller.DeleteAccount(existingAccount.Id);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [TestMethod]
    public async Task DeleteAccount_NonExistingId_ShouldReturnNotFound()
    {
        // Act
        var result = await Controller.DeleteAccount(999);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }
}
