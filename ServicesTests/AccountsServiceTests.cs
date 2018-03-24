using System.Collections.Generic;
using System.Linq;
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
    public class AccountsServiceTests
    {
        private Mock<IAccountsRepository> _mockAccountsRepository;
        private readonly IMapper _mapper = GetMapper();
        private AccountsService service;

        private readonly IEnumerable<AccountEntity> _testAccountEntities = GetTestAccountEntities();

        [TestInitialize]
        public void TestInitialize()
        {
            _mockAccountsRepository = new Mock<IAccountsRepository>();
            service = new AccountsService(_mockAccountsRepository.Object, _mapper);

            SetupMockAccountsRepository();
        }

        [TestMethod]
        public async Task GetAccounts_WhenCalled_ReturnsAccountEnumaration()
        {
            // Act
            var result = await service.GetAccounts();
            var expectedType = typeof(IEnumerable<Account>);

            // Assert
            Assert.IsInstanceOfType(result, expectedType);
        }

        [TestMethod]
        public async Task GetAccounts_WhenCalled_AccountsRepositoryGetAccountEntitiesIsCalledOnce()
        {
            // Act
            await service.GetAccounts();

            // Assert
            _mockAccountsRepository.Verify(r => r.GetAccountEntities(), Times.Once);
        }

        [TestMethod]
        public async Task GetAccount_WhenCalled_ReturnsAccountType()
        {
            // Act
            var result = await service.GetAccount(accountId: 1);
            var expectedType = typeof(Account);

            // Assert
            Assert.IsInstanceOfType(result, expectedType);
        }

        [TestMethod]
        public async Task GetAccount_WhenCalled_AccountsRepositoryGetAccountIsCalledOnce()
        {
            // Act
            await service.GetAccount(accountId: 1);

            // Assert
            _mockAccountsRepository.Verify(r => r.GetAccountEntity(1), Times.Once);
        }

        private void SetupMockAccountsRepository()
        {
            _mockAccountsRepository.Setup(r => r.GetAccountEntities()).Returns(Task.FromResult(_testAccountEntities));
            _mockAccountsRepository.Setup(r => r.GetAccountEntity(It.IsAny<int>())).Returns(Task.FromResult(_testAccountEntities.First()));
            _mockAccountsRepository.Setup(r => r.UpdateAccount(It.IsAny<AccountEntity>())).Verifiable();
        }

        private static IMapper GetMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(typeof(MapperProfiles)));
            var mapper = mapperConfig.CreateMapper();

            return mapper;
        }

        private static IEnumerable<AccountEntity> GetTestAccountEntities()
        {
            return new List<AccountEntity>()
            {
                new AccountEntity{Id = 1, Balance = 3},
                new AccountEntity{Id = 2, Balance = 5}
            };
        }
    }
}
