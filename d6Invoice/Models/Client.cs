namespace d6Invoice.Models
{
  public class Client
  {
    public int    Id      { get; set; }
    public string Name    { get; set; }
    public string Address { get; set; }
    public string Suburb  { get; set; }
    public string State   { get; set; }
    public string ZipCode { get; set; }
  }
}