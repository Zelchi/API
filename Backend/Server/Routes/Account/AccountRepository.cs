using Microsoft.EntityFrameworkCore;
using Database;
using Database.Models;

namespace Backend.Server.Routes.Account;

public class AccountRepository(Context Context)
{
    public async Task<IEnumerable<AccountEntity>> GetAll()
    {
        return await Context.Accounts.Where(a => a.DeletedAt == DateTime.MinValue).ToListAsync();
    }

    public async Task<AccountEntity> GetById(Guid id)
    {
        return await Context.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.DeletedAt == DateTime.MinValue);
    }

    public async Task<AccountEntity> GetByEmail(string email)
    {
        return await Context.Accounts.FirstOrDefaultAsync(a => a.Email == email && a.DeletedAt == DateTime.MinValue);
    }

    public async Task<AccountEntity> Create(AccountEntity account)
    {
        account.CreatedAt = DateTime.UtcNow;
        account.UpdatedAt = DateTime.UtcNow;
        account.DeletedAt = DateTime.MinValue;

        Context.Accounts.Add(account);
        await Context.SaveChangesAsync();
        return account;
    }

    public async Task<AccountEntity> Update(AccountEntity account)
    {
        account.UpdatedAt = DateTime.UtcNow;
        Context.Accounts.Update(account);
        await Context.SaveChangesAsync();
        return account;
    }

    public async Task DeleteAsync(Guid id)
    {
        var account = await GetById(id);
        if (account is not null)
        {
            account.DeletedAt = DateTime.UtcNow;
            account.UpdatedAt = DateTime.UtcNow;
            await Update(account);
        }
    }

    public async Task<bool> Exists(Guid id)
    {
        return await Context.Accounts.AnyAsync(a => a.Id == id && a.DeletedAt == DateTime.MinValue);
    }

    public async Task<bool> EmailExists(string email)
    {
        return await Context.Accounts.AnyAsync(a => a.Email == email && a.DeletedAt == DateTime.MinValue);
    }
}
