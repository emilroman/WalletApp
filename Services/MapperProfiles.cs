using AutoMapper;
using DbAccess.Abstractions;
using Services.Abstractions;

namespace Services
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<Account, AccountEntity>();
            CreateMap<Transaction, TransactionEntity>();
        }
    }
}