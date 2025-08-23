using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Backend.Server.Config;

namespace Test;

[TestClass]
public abstract class TestBase : IDisposable
{
    protected Database _context;
    protected IConfiguration _configuration;

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

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        var options = new DbContextOptionsBuilder<Database>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new Database(options, _configuration);
        _context.Database.EnsureCreated();
    }

    [TestCleanup]
    public virtual void Cleanup()
    {
        _context?.Dispose();
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

        _context.Accounts.Add(testAccount);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
