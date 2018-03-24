using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbAccess.Abstractions;

namespace DbAccess
{
    public static class NotFakeDatabase
    {
        public static ConcurrentDictionary<int, AccountEntity> Accounts;
        public static ConcurrentBag<TransactionEntity> TransactionsHistory;

        static NotFakeDatabase()
        {
            Accounts = new ConcurrentDictionary<int, AccountEntity>();
            TransactionsHistory = new ConcurrentBag<TransactionEntity>();
        }

        public static void SeedDatabase(int numberOfAccounts = 5)
        {
            Console.WriteLine("Seeding the database...");
            var randomNumberGenerator = new Random();

            for (int i = 0; i < numberOfAccounts; i++)
            {
                var accountEntity = new AccountEntity
                {
                    Id = i,
                    Balance = randomNumberGenerator.Next(10, 100)
                };

                Accounts.TryAdd(accountEntity.Id, accountEntity);
            }
        }

        
        // Because databases
        public static Task AccessDatabase()
        {
            Console.WriteLine("Accessing the database...");
            return Task.Delay(300);
        }
    }
}