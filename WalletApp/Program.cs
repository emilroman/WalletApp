using System;
using System.Collections.Generic;
using AutoMapper;
using DbAccess;
using DbAccess.Abstractions;
using DbAccess.Types;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;
using Services.Types;

namespace WalletApp
{
    public class Program
    {
        private static IAccountsService accountsService;
        private static ITransactionsService transactionsService;

        public static void Main(string[] args)
        {
            NotFakeDatabase.SeedDatabase();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IAccountsRepository, AccountsRepository>()
                .AddSingleton<ITransactionsRepository, TransactionsRepository>()
                .AddSingleton<IAccountsService, AccountsService>()
                .AddSingleton<ITransactionsService, TransactionService>()
                .AddAutoMapper()
                .BuildServiceProvider();

            accountsService = serviceProvider.GetService<IAccountsService>();
            transactionsService = serviceProvider.GetService<ITransactionsService>();

            Console.WriteLine("Hi, welcome to your wallet app!");
            ShowAccounts();
            
            Console.WriteLine("Let's transfer some $");
            Console.WriteLine("Please enter the accountId to transfer from:");
            var fromAccount = int.Parse(Console.ReadLine());

            Console.WriteLine("Please enter the accountId to transfer to:");
            var toAccount = int.Parse(Console.ReadLine());

            Console.WriteLine("Please enter the ammount to transfer:");
            var ammount = int.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("Processing transaction...");
            var transaction = new Transaction
            {
                FromAccountId = fromAccount,
                ToAccountId = toAccount,
                Amount = ammount
            };

            try
            {
                transactionsService.ProcessTransaction(transaction).GetAwaiter().GetResult();
                Console.WriteLine("Transaction succeded.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction failed ({ex.Message}).");
            }
            

            ShowAccounts();
            Console.Read();
        }

        private static void ShowAccounts()
        {
            Console.WriteLine();
            Console.WriteLine("-------Your accounts--------");

            var accounts = accountsService.GetAccounts().GetAwaiter().GetResult();
            foreach (var account in accounts)
            {
                Console.WriteLine($"Account {account.Id} has balance = {account.Balance}");
            }

            Console.WriteLine();
        }
    }
}
