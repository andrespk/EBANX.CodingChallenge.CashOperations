namespace EBANX.CodingTest.CashOperations.Models
{
    public class AccountEvent
    {
        public string Type { get; set; }
        public int Origin { get; set; }
        public int Destination { get; set; }
        public double Amount { get; set; }
    }
}