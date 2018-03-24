using System;

namespace DbAccess.Abstractions
{
    public class TransactionEntity
    {
        public TransactionEntity()
        {
            Id = new Guid();
        }

        public Guid Id { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public double Amount { get; set; }
    }
}