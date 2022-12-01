using System;

namespace d6Invoice.Models;

public class InvoiceHeader
{
  public int      Id       { get; set; }
  public string   RefNum   { get; set; }
  public DateTime Date     { get; set; }
  public short    Status   { get; set; }
  public int      ClientId { get; set; }
}