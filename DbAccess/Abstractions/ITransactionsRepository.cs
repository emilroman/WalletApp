using System.Threading.Tasks;

namespace DbAccess.Abstractions
{
    public interface ITransactionsRepository
    {
        Task AddTransaction(TransactionEntity transaction);
    }
}