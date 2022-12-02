using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace d6Invoice.Models;

public class Client
{
  public int Id { get; set; }

  [Required]
  [DisplayName( "Name" )]
  [MaxLength( 256 )]
  public string Name { get; set; }

  [DisplayName( "Address" )]
  [MaxLength( 256 )]
  public string Address { get; set; }

  [DisplayName( "Suburb" )]
  [MaxLength( 256 )]
  public string Suburb { get; set; }

  [DisplayName( "State" )]
  [MaxLength( 256 )]
  public string State { get; set; }

  [DisplayName( "Zip Code" )]
  [MaxLength( 10 )]
  public string ZipCode { get; set; }
}

public class ClientIndexViewModel
{
  public int                   PageCount  { get; set; }
  public int?                  Page       { get; set; }
  public int?                  RecPerPage { get; set; } = 10;
  public IEnumerable< Client > Clients    { get; set; } = new List< Client >();
}