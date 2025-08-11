using PruebaTecnica.Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Application.Features.Commands.CreateTransaction
{
    public class CreateTransactionsCommand
    {
        public TransactionRequest TransactionRequest { get; set; }

        public CreateTransactionsCommand(TransactionRequest transactionRequest)
        {
            TransactionRequest = transactionRequest;
        }
    }
}
