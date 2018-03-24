using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Abstractions;
using Services.Abstractions;

namespace Services.Types
{
    public class AccountsService : IAccountsService
    {
        private readonly IAccountsRepository _accountsRepository;
        private readonly IMapper _mapper;

        public AccountsService(IAccountsRepository accountsRepository, IMapper mapper)
        {
            _accountsRepository = accountsRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Account>> GetAccounts()
        {
            Console.WriteLine("AccountsService: Getting all accounts.");
            var accountEntities = await _accountsRepository.GetAccountEntities();
            var accouts = _mapper.Map<IEnumerable<Account>>(accountEntities);

            return accouts;
        }

        public async Task<Account> GetAccount(int accountId)
        {
            Console.WriteLine($"AccountsService: Getting account with id {accountId}");
            var accountEntity = await _accountsRepository.GetAccountEntity(accountId);
            var account = _mapper.Map<Account>(accountEntity);

            return account;
        }

        public async Task UpdateAccount(Account account)
        {
            Console.WriteLine($"AccountsService: Updating account with id {account.Id}");
            var accountEntity = _mapper.Map<AccountEntity>(account);
            await _accountsRepository.UpdateAccount(accountEntity);
        }
    }
}
