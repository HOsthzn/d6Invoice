using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace d6Invoice.Models;

public class Invoice
{
  public int Id { get; set; }

  [Required] public int InvoiceHeaderId { get; set; }

  [Required] public DateTime Date { get; set; }

  [DefaultValue( false )] public bool IsPaid { get; set; }
}