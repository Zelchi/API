using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace Test;

[TestClass]
public sealed class DatabaseConnectionTests : TestBase
{
    [TestMethod]
    public void Database_ShouldConnect_Successfully()
    {
        // Act & Assert
        _context.Should().NotBeNull();
        _context.Database.CanConnect().Should().BeTrue();
    }

    [TestMethod]
    public void Database_ShouldHave_AllTables()
    {
        // Act & Assert
        _context.Accounts.Should().NotBeNull();
        _context.Products.Should().NotBeNull();
        _context.Contacts.Should().NotBeNull();
    }

    [TestMethod]
    public void Configuration_ShouldHave_RequiredSettings()
    {
        // Assert
        _configuration.Should().NotBeNull();
        _configuration.GetConnectionString("DefaultConnection").Should().NotBeNullOrEmpty();
        _configuration["JWT:Key"].Should().NotBeNullOrEmpty();
        _configuration["JWT:Issuer"].Should().NotBeNullOrEmpty();
        _configuration["JWT:Audience"].Should().NotBeNullOrEmpty();
        _configuration["JWT:ExpiryInMinutes"].Should().NotBeNullOrEmpty();
    }
}
