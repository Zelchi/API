using Backend.Server.Config;

namespace Backend.Server.Routes.Account;

public class AccountService(Database Context)
{
    public async Task<IEnumerable<AccountEntity>> GetAllAccounts()
    {
        
    }
}