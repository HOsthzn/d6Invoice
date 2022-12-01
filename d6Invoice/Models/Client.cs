using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace d6Invoice.Models;

public class Client
{
  public int Id { get; set; }

  [Required] [DisplayName( "Name" )] public string Name { get; set; }

  [DisplayName( "Address" )] public string Address { get; set; }

  [DisplayName( "Suburb" )] public string Suburb { get; set; }

  [DisplayName( "State" )] public string State { get; set; }

  [DisplayName( "Zip Code" )] public string ZipCode { get; set; }
}

public class ClientIndexViewModel
{
  public int                   PageCount  { get; set; }
  public int?                   Page       { get; set; }
  public int?                   RecPerPage { get; set; } = 10;
  public IEnumerable< Client > Clients    { get; set; } = new List< Client >();
}