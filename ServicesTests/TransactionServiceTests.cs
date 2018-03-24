using System;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services;
using Services.Abstractions;
using Services.Types;

namespace ServicesTests
{
    [TestClass]
    public class TransactionServiceTests
    {
        private Mock<IAccountsService> _mockAccountsService;
        private Mock<ITransactionsRepository> _mockTransactionsRepository;
        private readonly IMapper _mapper = GetMapper();
        private TransactionService _service;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockAccountsService = new Mock<IAccountsService>();
            _mockTransactionsRepository = new Mock<ITransactionsRepository>();

            _service = new TransactionService(_mockAccountsService.Object, _mockTransactionsRepository.Object, _mapper);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task ProcessTransaction_AmountIsZero_ExceptionIsThrown()
        {
            // Arrange
            var transaction = new Transaction();

            // Act
            await _service.ProcessTransaction(transaction);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task ProcessTransaction_FromAccountBalanceIsTooLow_ExceptionIsThrown()
        {
            // Arrange
            var fromAccount = new Account();
            var transaction = new Transaction(){ Amount = 1 };
            SetupMockAccountsService(returnAccount: fromAccount);

            // Act
            await _service.ProcessTransaction(transaction);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task ProcessTransaction_FromAccountAndToAccountAreTheSame_ExceptionIsThrown()
        {
            // Arrange
            var testAccount = new Account { Id = 1, Balance = 5 };
            var transaction = new Transaction
            {
                Amount = 1,
                FromAccountId = testAccount.Id,
                ToAccountId = testAccount.Id
            }; ;
            SetupMockAccountsService(returnAccount: testAccount);

            // Act
            await _service.ProcessTransaction(transaction);
        }

        private void SetupMockAccountsService(Account returnAccount)
        {
            _mockAccountsService.Setup(s => s.GetAccount(It.IsAny<int>())).Returns(Task.FromResult(returnAccount));
            _mockAccountsService.Setup(s => s.UpdateAccount(It.IsAny<Account>())).Verifiable();
        }

        private void SetupTransactionsRepository()
        {
            _mockTransactionsRepository.Setup(r => r.AddTransaction(It.IsAny<TransactionEntity>())).Verifiable();
        }

        private static IMapper GetMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(typeof(MapperProfiles)));
            var mapper = mapperConfig.CreateMapper();

            return mapper;
        }
    }
}
