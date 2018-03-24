using System;
using System.Threading.Tasks;
using DbAccess.Abstractions;

namespace DbAccess.Types
{
    public class TransactionsRepository : ITransactionsRepository
    {
        public async Task AddTransaction(TransactionEntity transaction)
        {
            await NotFakeDatabase.AccessDatabase();

            Console.WriteLine($"TransactionsRepo: Adding transaction with id {transaction.Id} tot the transaction history.");
            NotFakeDatabase.TransactionsHistory.Add(transaction);
            Console.WriteLine("Transaction processed.");
        }
    }
}