using EBANX.CodingChallenge.CashOperations.DTOs;
using EBANX.CodingTest.CashOperations.Models;

namespace EBANX.CodingTest.CashOperations.Interfaces
{
    public interface IAccountService
    {
        ResponseDTO HandleEvent(AccountEvent accountEvent);

        ResponseDTO GetBalance(string accountId);

        void Reset();
    }
}