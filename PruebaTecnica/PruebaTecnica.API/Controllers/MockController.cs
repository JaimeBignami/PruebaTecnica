using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Application.Features.Commands.CreateTransaction;
using PruebaTecnica.Application.Models.Requests;
using PruebaTecnica.Application.Models.Responses;

namespace PruebaTecnica.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MockController : ControllerBase
    {
        private readonly CreateTransactionsHandler _createHandler;
        private readonly ILogger<TransactionsController> _logger;


        public MockController(ILogger<TransactionsController> logger, CreateTransactionsHandler createHandler)
        {
            _logger = logger;
            _createHandler = createHandler;
        }

        [HttpPost("Simular")]
        public async Task<ActionResult<TransactionResponse>> Simular([FromBody] TransactionRequest request)
        {
            string isoCode;

            if (request.Amount > 1000000)
                isoCode = "51";
            else if (request.Pan.EndsWith("0000"))
                isoCode = "05";
            else if (request.Pan.EndsWith("9999"))
                isoCode = "91";
            else if (
                !DateTime.TryParseExact(
                    request.Expiry,
                    "MM/yy",
                    null,
                    System.Globalization.DateTimeStyles.None,
                    out var expiryDate
                ) || expiryDate < new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1)
            )
                isoCode = "87";
            else
                isoCode = "00";

            return Ok(new { isoCode });
        }
    }
}
