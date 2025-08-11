using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using PruebaTecnica.Application.Contracts.Infrastructure;
using PruebaTecnica.Application.Contracts.Persistence;
using PruebaTecnica.Application.Features.Commands.CreateTransaction;
using PruebaTecnica.Application.Models.Requests;
using PruebaTecnica.Domain.Entities;
using Xunit;

namespace PruebaTecnica.Tests.Features.Commands.CreateTransactions
{
    public class CreateTransactionsHandlerTest
    {
        private readonly Mock<ITransactionsRepository> _transactionsRepositoryMock;
        private readonly CreateTransactionsHandler _handler;
        private readonly Mock<ILogger<CreateTransactionsHandler>> _loggerMock;
        private readonly Mock<IMockService> _mock;

        public CreateTransactionsHandlerTest()
        {
            _transactionsRepositoryMock = new Mock<ITransactionsRepository>();
            _loggerMock = new Mock<ILogger<CreateTransactionsHandler>>();
            _mock = new Mock<IMockService>();
            _handler = new CreateTransactionsHandler(_transactionsRepositoryMock.Object, _loggerMock.Object, _mock.Object);

        }

        [Fact]
        public async Task Handle_ValidTransactionRequest_ReturnsApprovedResponse()
        {
            var transactionRequest = new TransactionRequest
            {
                Pan = "4532015112830366",
                Cvv = "123",
                Expiry = "12/30",
                Amount = 10000,
                Currency = "CLP",
                MerchantId = "1"
            };
            var command = new CreateTransactionsCommand(transactionRequest);

            _mock.Setup(m => m.SimulateAcquirerResponse(It.IsAny<TransactionRequest>()))
            .ReturnsAsync("00");

            _transactionsRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Transactions>()))
                .ReturnsAsync((Transactions t) => t);
            // Act
            var response = await _handler.Handle(command);
            // Assert
            Assert.NotNull(response);
            Assert.Equal("00", response.AuthorizationCode); // Approved
            _transactionsRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transactions>()), Times.Once);
        }



        [Fact]
        public async Task Handle_PanEndsWith0000_ReturnsRejectedResponse()
        {
            // Arrange
            var transactionRequest = new TransactionRequest
            {
                Pan = "4532015112830000", // Termina en 0000 para forzar código 05
                Cvv = "123",
                Expiry = "12/30",
                Amount = 10000,
                Currency = "CLP",
                MerchantId = "1"
            };
            var command = new CreateTransactionsCommand(transactionRequest);

            _mock.Setup(m => m.SimulateAcquirerResponse(It.IsAny<TransactionRequest>()))
            .ReturnsAsync("05");

            _transactionsRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Transactions>()))
                .ReturnsAsync((Transactions t) => t);

            // Act
            var response = await _handler.Handle(command);

            // Assert
            Assert.NotNull(response);
            Assert.Equal("05", response.AuthorizationCode); // Rechazado
            Assert.Equal("Rechazado", response.Status);
            _transactionsRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transactions>()), Times.Once);
        }

        [Fact]
        public async Task Handle_AmountGreaterThanOneMillion_ReturnsInsufficientFundsResponse()
        {
            var transactionRequest = new TransactionRequest
            {
                Pan = "4532015112830366",
                Cvv = "123",
                Expiry = "12/30",
                Amount = 1000001, // Mayor a 1 millón para forzar código 51
                Currency = "CLP",
                MerchantId = "1"
            };
            var command = new CreateTransactionsCommand(transactionRequest);

            _mock.Setup(m => m.SimulateAcquirerResponse(It.IsAny<TransactionRequest>()))
            .ReturnsAsync("51");

            _transactionsRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Transactions>()))
                .ReturnsAsync((Transactions t) => t);

            var response = await _handler.Handle(command);

            Assert.NotNull(response);
            Assert.Equal("51", response.AuthorizationCode); // Fondos insuficientes
            Assert.Equal("Fondos insuficientes", response.Status);
            _transactionsRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transactions>()), Times.Once);
        }

        [Fact]
        public async Task Handle_PanEndsWith9999_ReturnsTemporaryErrorResponse()
        {
            var transactionRequest = new TransactionRequest
            {
                Pan = "4532015112839999", // Termina en 9999 para forzar código 91
                Cvv = "123",
                Expiry = "12/30",
                Amount = 10000,
                Currency = "CLP",
                MerchantId = "1"
            };
            var command = new CreateTransactionsCommand(transactionRequest);

            _mock.Setup(m => m.SimulateAcquirerResponse(It.IsAny<TransactionRequest>()))
            .ReturnsAsync("91");

            _transactionsRepositoryMock
                .Setup(repo => repo.CantidadReintentos(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), "91"))
                .ReturnsAsync(0);

            _transactionsRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Transactions>()))
                .ReturnsAsync((Transactions t) => t);

            var response = await _handler.Handle(command);

            Assert.NotNull(response);
            Assert.Equal("91", response.AuthorizationCode); // Error temporal
            Assert.Equal("Error temporal", response.Status);
            _transactionsRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transactions>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ExpiredCard_ReturnsNotAuthorizedResponse()
        {
            var expiredDate = DateTime.UtcNow.AddMonths(-1).ToString("MM/yy").Replace("-", "/");
            var transactionRequest = new TransactionRequest
            {
                Pan = "4532015112830366",
                Cvv = "123",
                Expiry = expiredDate, // Fecha expirada para forzar código 87
                Amount = 10000,
                Currency = "CLP",
                MerchantId = "1"
            };
            var command = new CreateTransactionsCommand(transactionRequest);

            _mock.Setup(m => m.SimulateAcquirerResponse(It.IsAny<TransactionRequest>()))
            .ReturnsAsync("87");

            _transactionsRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Transactions>()))
                .ReturnsAsync((Transactions t) => t);

            var response = await _handler.Handle(command);

            Assert.NotNull(response);
            Assert.Equal("87", response.AuthorizationCode); // No autorizado
            Assert.Equal("No autorizado", response.Status);
            _transactionsRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transactions>()), Times.Once);
        }
    }
}
