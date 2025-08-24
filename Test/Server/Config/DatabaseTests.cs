using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace Test.Server.Config;

[TestClass]
public sealed class DatabaseTests : TestBase
{
    [TestMethod]
    public void Database_ShouldConnect_Successfully()
    {
        // Act & Assert
        Context.Should().NotBeNull();
        Context.Database.CanConnect().Should().BeTrue();
    }

    [TestMethod]
    public void Database_ShouldHave_AllTables()
    {
        // Act & Assert
        Context.Accounts.Should().NotBeNull();
        Context.Products.Should().NotBeNull();
    }

    [TestMethod]
    public void Configuration_ShouldHave_RequiredSettings()
    {
        // Assert
        Configuration.Should().NotBeNull();
        Configuration.GetConnectionString("DefaultConnection").Should().NotBeNullOrEmpty();
        Configuration["JWT:Key"].Should().NotBeNullOrEmpty();
        Configuration["JWT:Issuer"].Should().NotBeNullOrEmpty();
        Configuration["JWT:Audience"].Should().NotBeNullOrEmpty();
        Configuration["JWT:ExpiryInMinutes"].Should().NotBeNullOrEmpty();
    }
}
