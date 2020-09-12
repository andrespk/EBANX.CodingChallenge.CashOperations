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
        public async Task<IActionResult> SetEvent([FromBody] AccountEvent model)
        {
            var response = await Task.FromResult(_service.HandleEvent(model));
            return StatusCode(response.StatusCode, response.Data);
        }

        [HttpGet]
        [Route("/balance")]
        public async Task<IActionResult> GetBalance([FromQuery] string account_id)
        {
            var response = await Task.FromResult(_service.GetBalance(account_id));
            return StatusCode(response.StatusCode, response.Data);
        }

        [HttpPost]
        [Route("/reset")]
        public void ResetBalance()
        {
            _service.Reset();
        }
    }
}