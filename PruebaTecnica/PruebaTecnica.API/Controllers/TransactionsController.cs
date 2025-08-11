using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Application.Features.Commands.CreateTransaction;
using PruebaTecnica.Application.Models.Requests;
using PruebaTecnica.Application.Models.Responses;

namespace PruebaTecnica.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly CreateTransactionsHandler _createHandler;
        private readonly ILogger<TransactionsController> _logger;


        public TransactionsController(ILogger<TransactionsController> logger, CreateTransactionsHandler createHandler)
        {
            _logger = logger;
            _createHandler = createHandler;
        }

        [HttpPost("IngresarTransaccion")]
        public async Task<ActionResult<TransactionResponse>> IngresarTransaccion(TransactionRequest transactionRequest)
        {
            try
            {
                var command = new CreateTransactionsCommand(transactionRequest);
                return await _createHandler.Handle(command);
            }       
            catch (ArgumentException ex)
            {
                return StatusCode(
                    StatusCodes.Status400BadRequest,
                    new TransactionResponse());
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new TransactionResponse());
            }
        }
    }
}
