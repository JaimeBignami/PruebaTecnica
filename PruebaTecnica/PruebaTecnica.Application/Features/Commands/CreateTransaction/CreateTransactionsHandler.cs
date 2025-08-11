using Microsoft.Extensions.Logging;
using PruebaTecnica.Application.Contracts.Infrastructure;
using PruebaTecnica.Application.Contracts.Persistence;
using PruebaTecnica.Application.Models.Requests;
using PruebaTecnica.Application.Models.Responses;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Application.Features.Commands.CreateTransaction
{
    public class CreateTransactionsHandler
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly CreateTransactionsValidator _validator;
        private readonly ILogger<CreateTransactionsHandler> _logger;
        private readonly IMockService _mockService;

        public CreateTransactionsHandler(
            ITransactionsRepository transactionsRepository, 
            ILogger<CreateTransactionsHandler> logger,
            IMockService mockService)
        {
            _transactionsRepository = transactionsRepository;
            _validator = new CreateTransactionsValidator();
            _logger = logger;
            _mockService = mockService;
        }

        public async Task<TransactionResponse> Handle(CreateTransactionsCommand command)
        {
            _logger.LogInformation("Procesando transacción: {Pan}, Monto: {Amount}, Moneda: {Currency}",
                command.TransactionRequest.Pan, command.TransactionRequest.Amount, command.TransactionRequest.Currency);
            string isoCode;
            var validationResult = _validator.Validate(command);
            if (!validationResult.IsValid)
            {
                // Si la validación falla, registramos el intento con código "05" (Rechazado)
                isoCode = "05";
                await RegistrarTransaccion(command.TransactionRequest, isoCode);
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogError("Validación fallida: {Errors}", errors);
                throw new ArgumentException($"Validación fallida: {errors}");
            }

            isoCode = await _mockService.SimulateAcquirerResponse(command.TransactionRequest);

            if (isoCode == "91")
            {
                var cantidadReintentos = await _transactionsRepository.CantidadReintentos(
                    command.TransactionRequest.Pan,
                    command.TransactionRequest.Cvv,
                    command.TransactionRequest.Expiry,
                    isoCode);

                if (cantidadReintentos >= 3)
                {
                    await RegistrarTransaccion(command.TransactionRequest, "05");
                    _logger.LogWarning("Reintentos excedidos para PAN: {Pan}, CVV: {Cvv}, Expiry: {Expiry}", 
                        command.TransactionRequest.Pan, command.TransactionRequest.Cvv, command.TransactionRequest.Expiry);
                    return BuildResponse("05");
                }
            }

            await RegistrarTransaccion(command.TransactionRequest, isoCode);
            _logger.LogInformation("Transacción procesada con éxito: {Pan}, Código ISO: {IsoCode}",
                command.TransactionRequest.Pan, isoCode);

            var response = BuildResponse(isoCode);
            return response;
        }

        private async Task RegistrarTransaccion(TransactionRequest request, string isoCode)
        {
            var transaction = new Transactions
            {
                Pan = request.Pan,
                Expiry = request.Expiry,
                Amount = request.Amount,
                Currency = request.Currency,
                Cvv = request.Cvv,
                MerchantId = request.MerchantId,
                AuthorizationCode = isoCode,
                CreatedAt = DateTime.UtcNow
            };
            await _transactionsRepository.AddAsync(transaction);
        }

        private TransactionResponse BuildResponse(string isoCode)
        {
            string status = "";
            switch (isoCode)
            {
                case "00":
                    status = "Aprobado";
                    break;
                case "05":
                    status = "Rechazado";
                    break;
                case "51":
                    status = "Fondos insuficientes";
                    break;
                case "91":
                    status = "Error temporal";
                    break;
                case "87":
                    status = "No autorizado";
                    break;
                default:
                    status = "Error desconocido";
                    break;
            }
            return new TransactionResponse { AuthorizationCode = isoCode, Status = status };
        }
    }
}
