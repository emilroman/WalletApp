namespace Services.Abstractions
{
    public class Transaction
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public double Amount { get; set; }
    }
}