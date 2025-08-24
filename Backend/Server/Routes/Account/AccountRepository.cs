using Microsoft.EntityFrameworkCore;
using Backend.Server.Config;

namespace Backend.Server.Routes.Account;

public class AccountRepository(Database Context)
{
    public async Task<IEnumerable<AccountEntity>> GetAllAsync()
    {
        return await Context.Accounts
            .Where(a => a.DeletedAt == DateTime.MinValue)
            .ToListAsync();
    }

    public async Task<AccountEntity> GetByIdAsync(int id)
    {
        return await Context.Accounts
            .FirstOrDefaultAsync(a => a.Id == id && a.DeletedAt == DateTime.MinValue);
    }

    public async Task<AccountEntity> GetByEmailAsync(string email)
    {
        return await Context.Accounts
            .FirstOrDefaultAsync(a => a.Email == email && a.DeletedAt == DateTime.MinValue);
    }

    public async Task<AccountEntity> CreateAsync(AccountEntity account)
    {
        account.CreatedAt = DateTime.UtcNow;
        account.UpdatedAt = DateTime.UtcNow;
        account.DeletedAt = DateTime.MinValue;

        Context.Accounts.Add(account);
        await Context.SaveChangesAsync();
        return account;
    }

    public async Task<AccountEntity> UpdateAsync(AccountEntity account)
    {
        account.UpdatedAt = DateTime.UtcNow;
        Context.Accounts.Update(account);
        await Context.SaveChangesAsync();
        return account;
    }

    public async Task DeleteAsync(int id)
    {
        var account = await GetByIdAsync(id);
        if (account != null)
        {
            account.DeletedAt = DateTime.UtcNow;
            account.UpdatedAt = DateTime.UtcNow;
            await UpdateAsync(account);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await Context.Accounts
            .AnyAsync(a => a.Id == id && a.DeletedAt == DateTime.MinValue);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await Context.Accounts
            .AnyAsync(a => a.Email == email && a.DeletedAt == DateTime.MinValue);
    }
}
