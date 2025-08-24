using Backend.Server.Tools;

namespace Backend.Server.Routes.Account;

public class AccountService(AccountRepository AccountRepository, IConfiguration Configuration)
{
    public async Task<IEnumerable<AccountEntity>> GetAll()
    {
        var accounts = await AccountRepository.GetAll();
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
        var account = await AccountRepository.GetById(id) ?? throw new Exception($"Conta com ID {id} não encontrada");
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
        var emailExists = await AccountRepository.EmailExists(createAccountDto.Email);
        if (emailExists)
            throw new Exception("Email já está em uso");

        var account = new AccountEntity
        {
            Username = createAccountDto.Username,
            Email = createAccountDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(createAccountDto.Password),
            Role = createAccountDto.Role
        };

        var createdAccount = await AccountRepository.Create(account);

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
        var account = await AccountRepository.GetById(id) ?? throw new Exception($"Conta com ID {id} não encontrada");
        account.Username = updateAccountDto.Username ?? account.Username;
        if (!string.IsNullOrEmpty(updateAccountDto.Password))
            account.Password = BCrypt.Net.BCrypt.HashPassword(updateAccountDto.Password);
        account.Role = updateAccountDto.Role ?? account.Role;

        var updatedAccount = await AccountRepository.Update(account);

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
        var exists = await AccountRepository.Exists(id);
        if (!exists)
            throw new Exception($"Conta com ID {id} não encontrada");

        await AccountRepository.DeleteAsync(id);
    }

    public async Task<LoginResponseDto> Login(LoginDto loginDto)
    {
        var account = await AccountRepository.GetByEmail(loginDto.Email) ?? throw new Exception("Email ou senha inválidos");
        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, account.Password)) throw new Exception("Email ou senha inválidos");

        string token = TokenGenerator.GenerateJwtToken(account, Configuration);

        return new LoginResponseDto
        {
            Token = token,
            Username = account.Username
        };
    }
}