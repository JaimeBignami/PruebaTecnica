using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Application.Models.Requests
{
    public class TransactionRequest
    {
        public string Pan { get; set; }
        public string Expiry { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Cvv { get; set; }
        public string MerchantId { get; set; }
    }
}
