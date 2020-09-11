using EBANX.CodingTest.CashOperations.Interfaces;
using EBANX.CodingTest.CashOperations.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EBANX.CodingTest.CashOperations.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IAccountService _service;

        public EventController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AccountEvent model)
        => await Task.FromResult(_service.HandleEvent(model));
    }
}