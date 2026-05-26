using System;
using System.Collections.Generic;

namespace POS.Database.Entities;

public partial class Sale
{
    public int Id { get; set; }

    public string InvoiceNo { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public DateTime SaleDate { get; set; }

    public int UserId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<VoidLog> VoidLogs { get; set; } = new List<VoidLog>();
}
