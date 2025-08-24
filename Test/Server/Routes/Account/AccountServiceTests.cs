using Backend.Server.Routes.Account;
using FluentAssertions;
using Test.Server.Config;

namespace Test.Server.Routes.Account;

[TestClass]
public class AccountServiceTests : TestBase
{
    private AccountService _accountService;

    [TestInitialize]
    public override void Setup()
    {
        base.Setup();
        var accountRepository = new AccountRepository(_context);
        _accountService = new AccountService(accountRepository, _configuration);
    }

    [TestMethod]
    public async Task Create_ValidAccount_ShouldReturnCreatedAccount()
    {
        // Arrange
        var createDto = new CreateAccountDto
        {
            Username = "newuser",
            Email = "newuser@test.com",
            Password = "password123",
            Role = "User"
        };

        // Act
        var result = await _accountService.Create(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Username.Should().Be("newuser");
        result.Email.Should().Be("newuser@test.com");
        result.Role.Should().Be("User");
        result.Id.Should().BeGreaterThan(0);
        
        var createdAccount = await _context.Accounts.FindAsync(result.Id);
        createdAccount.Should().NotBeNull();
        createdAccount.Username.Should().Be("newuser");
    }

    [TestMethod]
    public async Task Create_DuplicateEmail_ShouldThrowException()
    {
        // Arrange
        SeedTestData();
        var createDto = new CreateAccountDto
        {
            Username = "anotheruser",
            Email = "test@test.com", 
            Password = "password123",
            Role = "User"
        };

        // Act & Assert
        var action = async () => await _accountService.Create(createDto);
        await action.Should().ThrowAsync<Exception>()
            .WithMessage("Email já está em uso");
    }

    [TestMethod]
    public async Task GetAll_ShouldReturnAllAccounts()
    {
        // Arrange
        SeedTestData();

        // Act
        var result = await _accountService.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(0);
    }

    [TestMethod]
    public async Task GetById_ExistingId_ShouldReturnAccount()
    {
        // Arrange
        SeedTestData();
        var existingAccount = _context.Accounts.First();

        // Act
        var result = await _accountService.GetById(existingAccount.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(existingAccount.Id);
        result.Username.Should().Be(existingAccount.Username);
    }

    [TestMethod]
    public async Task GetById_NonExistingId_ShouldThrowException()
    {
        // Arrange
        var nonExistingId = 999;

        // Act & Assert
        var action = async () => await _accountService.GetById(nonExistingId);
        await action.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task Login_ValidCredentials_ShouldReturnToken()
    {
        // Arrange
        SeedTestData();
        var loginDto = new LoginDto
        {
            Email = "test@test.com",
            Password = "password123"
        };

        // Act
        var result = await _accountService.Login(loginDto);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
        result.User.Should().NotBeNull();
        result.User.Email.Should().Be("test@test.com");
    }

    [TestMethod]
    public async Task Login_InvalidEmail_ShouldThrowException()
    {
        // Arrange
        SeedTestData();
        var loginDto = new LoginDto
        {
            Email = "invalid@test.com",
            Password = "password123"
        };

        // Act & Assert
        var action = async () => await _accountService.Login(loginDto);
        await action.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task Login_InvalidPassword_ShouldThrowException()
    {
        // Arrange
        SeedTestData();
        var loginDto = new LoginDto
        {
            Email = "test@test.com",
            Password = "wrongpassword"
        };

        // Act & Assert
        var action = async () => await _accountService.Login(loginDto);
        await action.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task Update_ValidData_ShouldUpdateAccount()
    {
        // Arrange
        SeedTestData();
        var existingAccount = _context.Accounts.First();
        var updateDto = new UpdateAccountDto
        {
            Username = "updateduser",
            Role = "Admin"
        };

        // Act
        var result = await _accountService.Update(existingAccount.Id, updateDto);

        // Assert
        result.Should().NotBeNull();
        result.Username.Should().Be("updateduser");
        result.Role.Should().Be("Admin");
    }

    [TestMethod]
    public async Task Delete_ExistingAccount_ShouldMarkAsDeleted()
    {
        // Arrange
        SeedTestData();
        var existingAccount = _context.Accounts.First();

        // Act
        await _accountService.Delete(existingAccount.Id);

        // Assert
        _context.Entry(existingAccount).Reload();
        existingAccount.DeletedAt.Should().NotBe(DateTime.MinValue);
    }
}
