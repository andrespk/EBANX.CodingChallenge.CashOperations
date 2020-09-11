using EBANX.CodingTest.CashOperations.Models;
using Microsoft.AspNetCore.Mvc;

namespace EBANX.CodingTest.CashOperations.Interfaces
{
    public interface IAccountService
    {
        IActionResult HandleEvent(AccountEvent accountEvent);

        IActionResult GetBalance(int accountId);

        IActionResult Reset();
    }
}