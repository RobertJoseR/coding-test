using coding_test.Command;
using coding_test.Services;
using Microsoft.AspNetCore.Mvc;

namespace coding_test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly AccountService service;

        public BankAccountController(AccountService service)
        {
            this.service = service;
        }

        [HttpGet("{clientNumber}")]
        public async Task<IActionResult> Get([FromRoute] int clientNumber)
        {
            var results = await service.GetCustomer(clientNumber);

            return Ok(results);
        }

        [HttpGet("accounts/{clientNumber}")]
        public async Task<IActionResult> GetAccounts([FromRoute] int clientNumber)
        {
            var results = await service.GetAccounts(clientNumber);

            return Ok(results);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateAccount([FromForm] CreateAccount command)
        {

            var results = await service.CreateAccount(command);


            return results.IsSuccess
                    ? CreatedAtAction(nameof(Get), results.ResponseObject)
                    : BadRequest(results.ExceptionMessage);
        }


        [HttpDelete("account/{accountNumber}")]
        public async Task<IActionResult> RemoveAccount([FromRoute] int accountNumber)
        {
            var results = await service.RemoveAccount(accountNumber);

            return results.IsSuccess
                   ? Ok(results)
                   : BadRequest(results.ExceptionMessage);
        }
    }
}