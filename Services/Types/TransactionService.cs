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
        private readonly SemaphoreSlim _transactionSemaphore;

        public TransactionService(IAccountsService accountsService, ITransactionsRepository transactionsRepository, IMapper mapper)
        {
            _accountsService = accountsService;
            _transactionsRepository = transactionsRepository;
            _mapper = mapper;
            _transactionSemaphore = new SemaphoreSlim(1, 1);
        }

        public async Task ProcessTransaction(Transaction transaction)
        {
            ValidateTransactionAmount(transaction);
            ValidateTransactionAccounts(transaction);

            Console.WriteLine("TransactionsService: Started processing transaction.");

            var fromAccount = await _accountsService.GetAccount(transaction.FromAccountId);
            ValidateAccountHasEnoughMoney(fromAccount.Balance, transaction.Amount);

            var toAccount = await _accountsService.GetAccount(transaction.ToAccountId);

            await _transactionSemaphore.WaitAsync();
            try
            {
                fromAccount = await _accountsService.GetAccount(transaction.FromAccountId);
                ValidateAccountHasEnoughMoney(fromAccount.Balance, transaction.Amount);

                toAccount = await _accountsService.GetAccount(transaction.ToAccountId);

                fromAccount.Balance -= transaction.Amount;
                await _accountsService.UpdateAccount(fromAccount);

                toAccount.Balance += transaction.Amount;
                await _accountsService.UpdateAccount(toAccount);

                var transactionEntity = _mapper.Map<TransactionEntity>(transaction);
                await _transactionsRepository.AddTransaction(transactionEntity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction failed ({ex.Message})");
                _transactionSemaphore.Release();
                throw;
            }
            finally
            {
                _transactionSemaphore.Release();
            }

        }

        private static void ValidateTransactionAmount(Transaction transaction)
        {
            if (Math.Abs(transaction.Amount) < 0.001)
            {
                throw new Exception($"Ups! It looks like you want to transfer {transaction.Amount}. Could it be a typo?");
            }
        }

        private static void ValidateTransactionAccounts(Transaction transaction)
        {
            if (transaction.FromAccountId == transaction.ToAccountId)
            {
                throw new Exception("FromAccount and ToAccount are the same. Typo?");
            }
        }

        private static void ValidateAccountHasEnoughMoney(double accountBalance, double debitAmount)
        {
            if (accountBalance < debitAmount)
            {
                throw new Exception("Not enough $ bro!");
            }
        }
    }
}
