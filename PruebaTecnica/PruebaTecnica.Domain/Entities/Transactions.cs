using System;
using System.Collections.Generic;

namespace PruebaTecnica.Domain.Entities;

public partial class Transactions
{
    public int IdCargo { get; set; }

    public string? Pan { get; set; }

    public string? Expiry { get; set; }

    public int Amount { get; set; }

    public string? Currency { get; set; }

    public string? AuthorizationCode { get; set; }

    public string? MerchantId { get; set; }

    public string? Cvv { get; set; }

    public DateTime CreatedAt { get; set; }
}
