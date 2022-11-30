using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace d6Invoice.Models
{
  public class Client
  {
    public int    Id      { get; set; }

    [Required]
    [DisplayName("Name")]
    public string Name    { get; set; }

    [DisplayName( "Address")]
    public string Address { get; set; }

    [DisplayName( "Suburb")]
    public string Suburb  { get; set; }

    [DisplayName( "State")]
    public string State   { get; set; }

    [DisplayName( "Zip Code")]
    public string ZipCode { get; set; }
  }
}