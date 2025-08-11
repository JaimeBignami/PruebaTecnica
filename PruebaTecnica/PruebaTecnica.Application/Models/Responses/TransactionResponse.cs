using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Application.Models.Responses
{
    public class TransactionResponse
    {
        public string AuthorizationCode { get; set; }
        public string Status { get; set; } // Ej: "Aprobado", "Rechazado", "Error"
    }
}
