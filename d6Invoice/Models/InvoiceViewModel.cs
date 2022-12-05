using System;
using System.Collections.Generic;

namespace d6Invoice.Models
{
  public class InvoiceViewModel
  {
    public int      Id              { get; set; }
    public int      InvoiceHeaderId { get; set; }
    public DateTime Date            { get; set; }
    public bool     IsPaid          { get; set; }

    public    InvoiceHeader                 InvoiceHeader { get; set; }
  }

  public class InvoiceIndexViewModel
  {
    public int                             PageCount  { get; set; }
    public int?                            Page       { get; set; }
    public int?                            RecPerPage { get; set; } = 10;
    public IEnumerable< InvoiceViewModel > Invoices    { get; set; } = new List< InvoiceViewModel >();
  }
}