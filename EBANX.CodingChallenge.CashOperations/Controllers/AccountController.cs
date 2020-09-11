using EBANX.CodingTest.CashOperations.Interfaces;
using EBANX.CodingTest.CashOperations.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EBANX.CodingTest.CashOperations.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("/event")]
        public async Task<IActionResult> SetEvent(AccountEvent model)
        {
            var response = await Task.FromResult(_service.HandleEvent(model));
            return response.Data != null ? StatusCode(response.StatusCode, response.Data) : StatusCode(response.StatusCode, null);
        }

        [HttpGet]
        [Route("/balance")]
        public async Task<IActionResult> GetBalance(int account_id)
        {
            var response = await Task.FromResult(_service.GetBalance(account_id));
            return response.Data != null ? StatusCode(response.StatusCode, response.Data) : StatusCode(response.StatusCode, null);
        }

        [HttpPost]
        [Route("/reset")]
        public async Task<IActionResult> ResetBalance()
        {
            var response = await Task.FromResult(_service.Reset());
            return StatusCode(response.StatusCode, null);
        }
    }
}