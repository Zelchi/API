using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Backend.Server.Config;

namespace Test.Server.Config;

[TestClass]
public abstract class TestBase : IDisposable
{
    protected Database Context;
    protected IConfiguration Configuration;

    [TestInitialize]
    public virtual void Setup()
    {
        var configData = new Dictionary<string, string>
        {
            {"ConnectionStrings:DefaultConnection", "Server=localhost;Database=test;Uid=test;Pwd=test;"},
            {"JWT:Key", "MyVerySecretKeyThatIsAtLeast256BitsLong!MyVerySecretKeyThatIsAtLeast256BitsLong!"},
            {"JWT:Issuer", "TestIssuer"},
            {"JWT:Audience", "TestAudience"},
            {"JWT:ExpiryInMinutes", "60"}
        };

        Configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        var options = new DbContextOptionsBuilder<Database>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new Database(options, Configuration);
        Context.Database.EnsureCreated();
    }

    [TestCleanup]
    public virtual void Cleanup()
    {
        Context?.Dispose();
    }

    protected void SeedTestData()
    {
        var testAccount = new Backend.Server.Routes.Account.AccountEntity
        {
            Username = "testuser",
            Email = "test@test.com",
            Password = BCrypt.Net.BCrypt.HashPassword("password123"),
            Role = "User",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Accounts.Add(testAccount);
        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context?.Dispose();
    }
}
