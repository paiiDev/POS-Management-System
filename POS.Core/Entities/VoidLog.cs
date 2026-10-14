using System;
using System.Collections.Generic;

namespace POS.Database.Entities;

public partial class VoidLog
{
    public int Id { get; set; }

    public int SaleId { get; set; }

    public string InvoiceNo { get; set; } = null!;

    public decimal VoidedAmount { get; set; }

    public string Reason { get; set; } = null!;

    public DateTime VoidedAt { get; set; }

    public string CashierName { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}
