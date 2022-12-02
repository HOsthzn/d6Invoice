using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace d6Invoice.Models;

public class InvoiceDetails
{
  public int Id              { get; set; }

  [Required]
  public int InvoiceHeaderId { get; set; }
  
  [Required]
  public int ProductId       { get; set; }

  [DefaultValue(0)]
  public int Quantity        { get; set; }
}