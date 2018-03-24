using System.Collections.Generic;
using System.Threading.Tasks;

namespace DbAccess.Abstractions
{
    public interface IAccountsRepository
    {
        Task<IEnumerable<AccountEntity>> GetAccountEntities();
        Task<AccountEntity> GetAccountEntity(int accountId);
        Task UpdateAccount(AccountEntity accountEntity);
    }
}