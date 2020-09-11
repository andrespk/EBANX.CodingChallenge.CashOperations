using EBANX.CodingTest.CashOperations.Interfaces;

namespace EBANX.CodingTest.CashOperations.Models
{
    public class Account : IAccount
    {
        protected double _balance;
        public int Id { get; private set; }

        public double Balance { get { return _balance; } }

        public Account(int id, double balance = 0)
        {
            Id = id;
            _balance = balance;
        }

        public void MakeDeposit(double amount)
        {
            _balance += amount;
        }

        public void MakeWithdraw(double amount)
        {
            _balance -= (amount > _balance ? _balance : amount);
        }

        public void MakeTransfer(double amount, ref Account destination)
        {
            var transferred = amount > _balance ? _balance : amount;
            MakeWithdraw(transferred);
            destination.MakeDeposit(transferred);
        }
    }
}