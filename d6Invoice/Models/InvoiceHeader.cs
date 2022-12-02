using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace d6Invoice.Models;

public class InvoiceHeader
{
  public int Id { get; set; }

  [Required] public string RefNum { get; set; }

  [Required] public DateTime Date { get; set; }

  [DefaultValue( 0 )] public short Status { get; set; }

  [Required] public int ClientId { get; set; }
}