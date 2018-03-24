using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface ITransactionsService
    {
        Task ProcessTransaction(Transaction transaction);
    }
}