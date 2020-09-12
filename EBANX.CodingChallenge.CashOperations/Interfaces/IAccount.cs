using EBANX.CodingTest.CashOperations.Models;

namespace EBANX.CodingTest.CashOperations.Interfaces
{
    public interface IAccount
    {
        public string Id { get; }
        public double Balance { get; }

        void MakeDeposit(double amount);

        void MakeWithdraw(double amount);

        void MakeTransfer(double amount, ref Account account);
    }
}