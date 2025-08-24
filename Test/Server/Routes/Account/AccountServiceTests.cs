using Backend.Server.Routes.Account;
using FluentAssertions;
using Test.Server.Config;

namespace Test.Server.Routes.Account;

[TestClass]
public class AccountServiceTests : TestBase
{
    private AccountService AccountService;

    [TestInitialize]
    public override void Setup()
    {
        base.Setup();
        var accountRepository = new AccountRepository(Context);
        AccountService = new AccountService(accountRepository, Configuration);
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
        var result = await AccountService.Create(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Username.Should().Be("newuser");
        result.Email.Should().Be("newuser@test.com");
        result.Role.Should().Be("User");
        result.Id.Should().BeGreaterThan(0);
        
        var createdAccount = await Context.Accounts.FindAsync(result.Id);
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
        var action = async () => await AccountService.Create(createDto);
        await action.Should().ThrowAsync<Exception>()
            .WithMessage("Email já está em uso");
    }

    [TestMethod]
    public async Task GetAll_ShouldReturnAllAccounts()
    {
        // Arrange
        SeedTestData();

        // Act
        var result = await AccountService.GetAll();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(0);
    }

    [TestMethod]
    public async Task GetById_ExistingId_ShouldReturnAccount()
    {
        // Arrange
        SeedTestData();
        var existingAccount = Context.Accounts.First();

        // Act
        var result = await AccountService.GetById(existingAccount.Id);

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
        var action = async () => await AccountService.GetById(nonExistingId);
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
        var result = await AccountService.Login(loginDto);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
        result.Username.Should().NotBeNull();
        result.Token.Split('.').Length.Should().Be(3);
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
        var action = async () => await AccountService.Login(loginDto);
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
        var action = async () => await AccountService.Login(loginDto);
        await action.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task Update_ValidData_ShouldUpdateAccount()
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
        var result = await AccountService.Update(existingAccount.Id, updateDto);

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
        var existingAccount = Context.Accounts.First();

        // Act
        await AccountService.Delete(existingAccount.Id);

        // Assert
        Context.Entry(existingAccount).Reload();
        existingAccount.DeletedAt.Should().NotBe(DateTime.MinValue);
    }
}
