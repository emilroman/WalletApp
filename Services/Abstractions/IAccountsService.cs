using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IAccountsService
    {
        Task<IEnumerable<Account>> GetAccounts();
        Task<Account> GetAccount(int accountId);
        Task UpdateAccount(Account accountEntity);
    }
}