using Backend.Server.Config;
using Microsoft.EntityFrameworkCore;

namespace Backend.Server.Routes.Account;

public class AccountService(Database Context)
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
                Password = a.Password,
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
                Password = a.Password,
                DeletedAt = a.DeletedAt,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            })
            .FirstOrDefaultAsync() ?? throw new Exception($"Conta com ID {id} não encontrada");

        return account;
    }

    public async Task<AccountEntity> Create(CreateAccountDto createAccountDto)
    {
        var account = new AccountEntity
        {
            Username = createAccountDto.Username,
            Email = createAccountDto.Email,
            Password = createAccountDto.Password,
            Role = createAccountDto.Role,
            DeletedAt = DateTime.MinValue,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Accounts.Add(account);
        await Context.SaveChangesAsync();

        return account;
    }

    public async Task<AccountEntity> Update(int id, UpdateAccountDto updateAccountDto)
    {
        var account = await Context.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.DeletedAt == DateTime.MinValue)
        ?? throw new Exception($"Conta com ID {id} não encontrada");

        account.Username = updateAccountDto.Username ?? account.Username;
        account.Password = updateAccountDto.Password ?? account.Password;
        account.Role = updateAccountDto.Role ?? account.Role;

        account.UpdatedAt = DateTime.UtcNow;

        Context.Accounts.Update(account);
        await Context.SaveChangesAsync();

        return account;
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
}