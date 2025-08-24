using Backend.Server.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Server.Routes.Account;

public class AccountService(Database Context, IConfiguration Configuration)
{
    public async Task<IEnumerable<AccountEntity>> GetAll()
    {
        var accounts = await Context.Accounts
            .Where(a => a.DeletedAt == DateTime.MinValue)
            .Select(a => new AccountEntity
            {
                Id = a.Id,
                Username = a.Username,
                Email = a.Email,
                Role = a.Role,
                DeletedAt = a.DeletedAt,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            })
            .ToListAsync();

        return accounts;
    }

    public async Task<AccountEntity> GetById(int id)
    {
        var account = await Context.Accounts
            .Where(a => a.Id == id && a.DeletedAt == DateTime.MinValue)
            .Select(a => new AccountEntity
            {
                Id = a.Id,
                Username = a.Username,
                Email = a.Email,
                Role = a.Role,
                DeletedAt = a.DeletedAt,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            })
            .FirstOrDefaultAsync() ?? throw new Exception($"Conta com ID {id} não encontrada");

        return account;
    }

    public async Task<AccountEntity> Create(CreateAccountDto createAccountDto)
    {
        var existingAccount = await Context.Accounts
            .FirstOrDefaultAsync(a => a.Email == createAccountDto.Email && a.DeletedAt == DateTime.MinValue);
        
        if (existingAccount != null)
            throw new Exception("Email já está em uso");

        var account = new AccountEntity
        {
            Username = createAccountDto.Username,
            Email = createAccountDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(createAccountDto.Password),
            Role = createAccountDto.Role,
            DeletedAt = DateTime.MinValue,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Accounts.Add(account);
        await Context.SaveChangesAsync();

        return new AccountEntity
        {
            Id = account.Id,
            Username = account.Username,
            Email = account.Email,
            Role = account.Role,
            DeletedAt = account.DeletedAt,
            CreatedAt = account.CreatedAt,
            UpdatedAt = account.UpdatedAt
        };
    }

    public async Task<AccountEntity> Update(int id, UpdateAccountDto updateAccountDto)
    {
        var account = await Context.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.DeletedAt == DateTime.MinValue)
        ?? throw new Exception($"Conta com ID {id} não encontrada");

        account.Username = updateAccountDto.Username ?? account.Username;
        if (!string.IsNullOrEmpty(updateAccountDto.Password))
            account.Password = BCrypt.Net.BCrypt.HashPassword(updateAccountDto.Password);
        account.Role = updateAccountDto.Role ?? account.Role;

        account.UpdatedAt = DateTime.UtcNow;

        Context.Accounts.Update(account);
        await Context.SaveChangesAsync();

        return new AccountEntity
        {
            Id = account.Id,
            Username = account.Username,
            Email = account.Email,
            Role = account.Role,
            DeletedAt = account.DeletedAt,
            CreatedAt = account.CreatedAt,
            UpdatedAt = account.UpdatedAt
        };
    }

    public async Task Delete(int id)
    {
        var account = await Context.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.DeletedAt == DateTime.MinValue)
        ?? throw new Exception($"Conta com ID {id} não encontrada");

        account.DeletedAt = DateTime.UtcNow;
        account.UpdatedAt = DateTime.UtcNow;

        Context.Accounts.Update(account);
        await Context.SaveChangesAsync();
    }

    public async Task<LoginResponseDto> Login(LoginDto loginDto)
    {
        var account = await Context.Accounts
            .FirstOrDefaultAsync(a => a.Email == loginDto.Email && a.DeletedAt == DateTime.MinValue)
            ?? throw new Exception("Email ou senha inválidos");

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, account.Password))
            throw new Exception("Email ou senha inválidos");

        var token = GenerateJwtToken(account);

        return new LoginResponseDto
        {
            Token = token,
            User = new AccountEntity
            {
                Id = account.Id,
                Username = account.Username,
                Email = account.Email,
                Role = account.Role,
                CreatedAt = account.CreatedAt,
                UpdatedAt = account.UpdatedAt
            }
        };
    }

    private string GenerateJwtToken(AccountEntity account)
    {
        var key = Configuration.GetSection("JWT").GetValue<string>("Key") ?? "DefaultSecretKeyThatIsAtLeast32CharactersLong";
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Role, account.Role)
            ]),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}