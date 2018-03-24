using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess;
using DbAccess.Abstractions;
using Services.Abstractions;

namespace Services.Types
{
    public class TransactionService : ITransactionsService
    {
        private readonly IAccountsService _accountsService;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IMapper _mapper;
        private readonly SemaphoreSlim _transactionLock;

        public TransactionService(IAccountsService accountsService, ITransactionsRepository transactionsRepository, IMapper mapper)
        {
            _accountsService = accountsService;
            _transactionsRepository = transactionsRepository;
            _mapper = mapper;
            _transactionLock = new SemaphoreSlim(1, 1);
        }

        public async Task ProcessTransaction(Transaction transaction)
        {
            if (Math.Abs(transaction.Amount) < 0.001)
            {
                throw new Exception($"Ups! It looks like you want to transfer {transaction.Amount}. Could it be a typo?");
            }

            Console.WriteLine("TransactionsService: Started processing transaction.");
            
            var fromAccount = await _accountsService.GetAccount(transaction.FromAccountId);
            if (!TrasactionIsPossible(fromAccount.Balance, transaction.Amount))
            {
                throw new Exception("Not enough $ bro!");
            }
            var toAccount = await _accountsService.GetAccount(transaction.ToAccountId);

            await _transactionLock.WaitAsync();
            try
            {
                if (TrasactionIsPossible(fromAccount.Balance, transaction.Amount))
                {
                    fromAccount.Balance -= transaction.Amount;
                    await _accountsService.UpdateAccount(fromAccount);

                    toAccount.Balance += transaction.Amount;
                    await _accountsService.UpdateAccount(toAccount);

                    var transactionEntity = _mapper.Map<TransactionEntity>(transaction);
                    await _transactionsRepository.AddTransaction(transactionEntity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction failed ({ex.Message})");
            }
            finally
            {
                _transactionLock.Release();
            }

        }

        private static bool TrasactionIsPossible(double accountBalance, double debitAmount)
        {
            return accountBalance >= debitAmount;
        }
    }
}
