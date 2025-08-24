using Backend.Server.Routes.Contact;
using Backend.Server.Routes.Account;
using FluentAssertions;
using Test.Server.Config;

namespace Test.Server.Routes.Contact;

[TestClass]
public class ContactServiceTests : TestBase
{
    private ContactService _contactService;
    private int _testAccountId = 1;

    [TestInitialize]
    public override void Setup()
    {
        base.Setup();
        var contactRepository = new ContactRepository(_context);
        _contactService = new ContactService(contactRepository);
    }

    private void SeedTestDataForContacts()
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

        var testContact = new ContactEntity
        {
            Name = "João Silva",
            Phone = "(11) 99999-9999",
            Email = "joao@test.com",
            AccountId = _testAccountId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Contacts.Add(testContact);
        _context.SaveChanges();
    }

    [TestMethod]
    public async Task Create_ValidContact_ShouldReturnCreatedContact()
    {
        // Arrange
        SeedTestDataForContacts();
        var createDto = new CreateContactDto
        {
            Name = "Maria Santos",
            Phone = "(11) 88888-8888",
            Email = "maria@test.com"
        };

        // Act
        var result = await _contactService.Create(createDto, _testAccountId);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Maria Santos");
        result.Phone.Should().Be("(11) 88888-8888");
        result.Email.Should().Be("maria@test.com");
        result.AccountId.Should().Be(_testAccountId);
        result.Id.Should().BeGreaterThan(0);

        var createdContact = await _context.Contacts.FindAsync(result.Id);
        createdContact.Should().NotBeNull();
        createdContact.Name.Should().Be("Maria Santos");
    }

    [TestMethod]
    public async Task GetAll_ShouldReturnUserContacts()
    {
        // Arrange
        SeedTestDataForContacts();
        
        // Act
        var result = await _contactService.GetAll(_testAccountId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(0);
        result.All(c => c.AccountId == _testAccountId).Should().BeTrue();
        result.All(c => c.DeletedAt == DateTime.MinValue).Should().BeTrue();
    }

    [TestMethod]
    public async Task GetById_ExistingContact_ShouldReturnContact()
    {
        // Arrange
        SeedTestDataForContacts();
        var existingContact = _context.Contacts.First(c => c.AccountId == _testAccountId);

        // Act
        var result = await _contactService.GetById(existingContact.Id, _testAccountId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(existingContact.Id);
        result.Name.Should().Be(existingContact.Name);
        result.AccountId.Should().Be(_testAccountId);
    }

    [TestMethod]
    public async Task GetById_NonExistingContact_ShouldThrowException()
    {
        // Arrange
        var nonExistingId = 999;

        // Act & Assert
        var action = async () => await _contactService.GetById(nonExistingId, _testAccountId);
        await action.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task GetById_ContactFromDifferentUser_ShouldThrowException()
    {
        // Arrange
        SeedTestDataForContacts();
        var otherAccount = new AccountEntity
        {
            Username = "otheruser",
            Email = "other@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("password123"),
            Role = "User"
        };
        _context.Accounts.Add(otherAccount);
        _context.SaveChanges();

        var existingContact = _context.Contacts.First(c => c.AccountId == _testAccountId);

        // Act & Assert
        var action = async () => await _contactService.GetById(existingContact.Id, otherAccount.Id);
        await action.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task Update_ValidData_ShouldUpdateContact()
    {
        // Arrange
        SeedTestDataForContacts();
        var existingContact = _context.Contacts.First(c => c.AccountId == _testAccountId);
        var updateDto = new UpdateContactDto
        {
            Name = "João Silva Atualizado",
            Phone = "(11) 77777-7777",
            Email = "joao.atualizado@test.com"
        };

        // Act
        var result = await _contactService.Update(existingContact.Id, updateDto, _testAccountId);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("João Silva Atualizado");
        result.Phone.Should().Be("(11) 77777-7777");
        result.Email.Should().Be("joao.atualizado@test.com");
        result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
    }

    [TestMethod]
    public async Task Update_PartialData_ShouldUpdateOnlyProvidedFields()
    {
        // Arrange
        SeedTestDataForContacts();
        var existingContact = _context.Contacts.First(c => c.AccountId == _testAccountId);
        var originalPhone = existingContact.Phone;
        var originalEmail = existingContact.Email;
        
        var updateDto = new UpdateContactDto
        {
            Name = "Apenas Nome Atualizado"
        };

        // Act
        var result = await _contactService.Update(existingContact.Id, updateDto, _testAccountId);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Apenas Nome Atualizado");
        result.Phone.Should().Be(originalPhone); 
        result.Email.Should().Be(originalEmail); 
    }

    [TestMethod]
    public async Task Update_NonExistingContact_ShouldThrowException()
    {
        // Arrange
        var updateDto = new UpdateContactDto
        {
            Name = "Contato Atualizado"
        };

        // Act & Assert
        var action = async () => await _contactService.Update(999, updateDto, _testAccountId);
        await action.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task Delete_ExistingContact_ShouldMarkAsDeleted()
    {
        // Arrange
        SeedTestDataForContacts();
        var existingContact = _context.Contacts.First(c => c.AccountId == _testAccountId);

        // Act
        await _contactService.Delete(existingContact.Id, _testAccountId);

        // Assert
        _context.Entry(existingContact).Reload();
        existingContact.DeletedAt.Should().NotBe(DateTime.MinValue);
        existingContact.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
    }

    [TestMethod]
    public async Task Delete_NonExistingContact_ShouldThrowException()
    {
        // Act & Assert
        var action = async () => await _contactService.Delete(999, _testAccountId);
        await action.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task Delete_ContactFromDifferentUser_ShouldThrowException()
    {
        // Arrange
        SeedTestDataForContacts();
        var otherAccount = new AccountEntity
        {
            Username = "otheruser",
            Email = "other@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("password123"),
            Role = "User"
        };
        _context.Accounts.Add(otherAccount);
        _context.SaveChanges();

        var existingContact = _context.Contacts.First(c => c.AccountId == _testAccountId);

        // Act & Assert
        var action = async () => await _contactService.Delete(existingContact.Id, otherAccount.Id);
        await action.Should().ThrowAsync<Exception>();
    }
}
