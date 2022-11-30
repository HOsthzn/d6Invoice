using System;

namespace d6Invoice.Models
{
  public class Invoice
  {
    public int      Id              { get; set; }
    public int      InvoiceHeaderId { get; set; }
    public DateTime Date            { get; set; }
    public bool     IsPaid          { get; set; }
  }
}