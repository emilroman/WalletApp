using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.Abstractions;

namespace DbAccess.Types
{
    public class AccountsRepository :  IAccountsRepository
    {
        public async Task<IEnumerable<AccountEntity>> GetAccountEntities()
        {
            Console.WriteLine("AccountsRepo: Getting all accounts.");

            await NotFakeDatabase.AccessDatabase();

            var accounts = NotFakeDatabase.Accounts.Values;

            if (!accounts.Any())
            {
                throw new Exception("AccountsRepo: The database currently contains no accounts.");
            }

            return accounts;
        }

        public async Task<AccountEntity> GetAccountEntity(int accountId)
        {
            Console.WriteLine($"AccountsRepo: Getting account with id {accountId}");

            await NotFakeDatabase.AccessDatabase();

            var account = GetAccount(accountId);

            return account;
        }

        public async Task UpdateAccount(AccountEntity accountEntity)
        {
            Console.WriteLine($"AccountsRepo: Updating account with id {accountEntity.Id}");

            await NotFakeDatabase.AccessDatabase();

            var account = GetAccount(accountEntity.Id);
            account.Balance = accountEntity.Balance;
        }

        private static AccountEntity GetAccount(int accountId)
        {
            var account = NotFakeDatabase.Accounts.FirstOrDefault(a => a.Key == accountId).Value;
            if (account == null)
            {
                throw new Exception($"AccountsRepo: No account with id {accountId} was found.");
            }

            return account;
        }
    }
}