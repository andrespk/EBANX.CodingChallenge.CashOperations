using EBANX.CodingTest.CashOperations.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EBANX.CodingTest.CashOperations.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IAccountService _service;

        public BalanceController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int account_id)
        => await Task.FromResult(_service.GetBalance(account_id));

        [HttpPost]
        [Route("reset")]
        public async Task<IActionResult> Reset()
        => await Task.FromResult(_service.Reset());
    }
}
}