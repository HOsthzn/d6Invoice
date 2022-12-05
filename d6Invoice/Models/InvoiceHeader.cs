using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace d6Invoice.Models;

public class InvoiceHeader
{
  public int Id { get; set; }

  [Required] public DateTime Date { get; set; }

  [DefaultValue( 0 )] public short Status { get; set; }

  [Required] public int ClientId { get; set; }

  [Required] public DataType DueDate { get; set; }

  public Client        Client { get; set; }
  public IEnumerable<InvoiceDetails> Details   { get; set; }

}