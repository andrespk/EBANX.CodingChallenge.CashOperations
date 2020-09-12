namespace EBANX.CodingTest.CashOperations.Models
{
    public class AccountEvent
    {
        public string Type { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Amount { get; set; }
    }
}