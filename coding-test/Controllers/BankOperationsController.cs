using coding_test.Command;
using coding_test.Services;
using Microsoft.AspNetCore.Mvc;

namespace coding_test.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BankOperationsController : ControllerBase
    {
        private readonly TransactionService service;

        public BankOperationsController(TransactionService service)
        {
            this.service = service;
        }


        [HttpPost("deposit")]
        [Produces("application/json")]
        public async Task<IActionResult> MakeDeposit([FromForm] BankTransactionCommand command)
        {
            var results = await service.MakeDeposit(command);
             
            return results.IsSuccess
                    ? CreatedAtAction($"bankaccount/{command.AccountNumber}", results)
                    : BadRequest(results.ExceptionMessage);
        }

        [HttpPost("withdrawal")]
        [Produces("application/json")]
        public async Task<IActionResult> Withdrawal([FromForm] BankTransactionCommand command)
        {
            var results = await service.WithDrawal(command);

            return results.IsSuccess
                    ? CreatedAtAction($"bankaccount/{command.AccountNumber}", results)
                    : BadRequest(results.ExceptionMessage);

        }
    }
}
