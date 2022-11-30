namespace d6Invoice.Models
{
  public class InvoiceDetails
  {
    public int Id              { get; set; }
    public int InvoiceHeaderId { get; set; }
    public int ProductId       { get; set; }
    public int Quantity        { get; set; }
  }
}