using PruebaTecnica.Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Application.Contracts.Infrastructure
{
    public interface IMockService
    {
        Task<string> SimulateAcquirerResponse(TransactionRequest request);
    }
}
