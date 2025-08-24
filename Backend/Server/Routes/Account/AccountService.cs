using Backend.Server.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Server.Routes.Account;

public class AccountService(AccountRepository accountRepository, IConfiguration configuration)
{
    private readonly AccountRepository _accountRepository = accountRepository;
    private readonly IConfiguration _configuration = configuration;

    public async Task<IEnumerable<AccountEntity>> GetAll()
    {
        var accounts = await _accountRepository.GetAllAsync();
        return accounts.Select(a => new AccountEntity
        {
            Id = a.Id,
            Username = a.Username,
            Email = a.Email,
            Role = a.Role,
            DeletedAt = a.DeletedAt,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        });
    }

    public async Task<AccountEntity> GetById(int id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null)
            throw new Exception($"Conta com ID {id} não encontrada");

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

    public async Task<AccountEntity> Create(CreateAccountDto createAccountDto)
    {
        var emailExists = await _accountRepository.EmailExistsAsync(createAccountDto.Email);
        if (emailExists)
            throw new Exception("Email já está em uso");

        var account = new AccountEntity
        {
            Username = createAccountDto.Username,
            Email = createAccountDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(createAccountDto.Password),
            Role = createAccountDto.Role
        };

        var createdAccount = await _accountRepository.CreateAsync(account);

        return new AccountEntity
        {
            Id = createdAccount.Id,
            Username = createdAccount.Username,
            Email = createdAccount.Email,
            Role = createdAccount.Role,
            DeletedAt = createdAccount.DeletedAt,
            CreatedAt = createdAccount.CreatedAt,
            UpdatedAt = createdAccount.UpdatedAt
        };
    }

    public async Task<AccountEntity> Update(int id, UpdateAccountDto updateAccountDto)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null)
            throw new Exception($"Conta com ID {id} não encontrada");

        account.Username = updateAccountDto.Username ?? account.Username;
        if (!string.IsNullOrEmpty(updateAccountDto.Password))
            account.Password = BCrypt.Net.BCrypt.HashPassword(updateAccountDto.Password);
        account.Role = updateAccountDto.Role ?? account.Role;

        var updatedAccount = await _accountRepository.UpdateAsync(account);

        return new AccountEntity
        {
            Id = updatedAccount.Id,
            Username = updatedAccount.Username,
            Email = updatedAccount.Email,
            Role = updatedAccount.Role,
            DeletedAt = updatedAccount.DeletedAt,
            CreatedAt = updatedAccount.CreatedAt,
            UpdatedAt = updatedAccount.UpdatedAt
        };
    }

    public async Task Delete(int id)
    {
        var exists = await _accountRepository.ExistsAsync(id);
        if (!exists)
            throw new Exception($"Conta com ID {id} não encontrada");

        await _accountRepository.DeleteAsync(id);
    }

    public async Task<LoginResponseDto> Login(LoginDto loginDto)
    {
        var account = await _accountRepository.GetByEmailAsync(loginDto.Email);
        if (account == null)
            throw new Exception("Email ou senha inválidos");

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
        var key = _configuration.GetSection("JWT").GetValue<string>("Key") ?? "DefaultSecretKeyThatIsAtLeast32CharactersLong";
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