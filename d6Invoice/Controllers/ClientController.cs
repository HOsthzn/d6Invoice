using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using d6Invoice.Models;
using d6Invoice.Utilities;

namespace d6Invoice.Controllers;

public class ClientController : Controller
{
  private readonly AdoNet _net;

  public ClientController()
  {
    _net = new AdoNet( ConfigurationManager.ConnectionStrings[ "DefaultConnection" ].ConnectionString );
  }

  //GET Client/index
  public ActionResult Index( int? recPerPage ) { return View(); }

  //GET Client/GetClients
  public async Task< ActionResult > GetClients( int? page, int? recPerPage )
  {
    //Get all Client records from the DB
    Hashtable parameters = new() { { "@page", page }, { "@recPerPage", recPerPage } };

    ClientIndexViewModel result = new()
                                  {
                                    Page       = page
                                  , RecPerPage = recPerPage
                                  };
    result.Clients = await _net.StpAsync< Client >( "Client_Get", parameters );
    result.PageCount = _net.StpAsync< PageCount >( "Client_GetPageCount"
                                                , new Hashtable { { "@recPerPage", recPerPage } } )
                           .Result.Count;
    
    return Json( result, JsonRequestBehavior.AllowGet );
  }

  //GET Client/details
  public async Task< ActionResult > Details( int id )
  {
    Hashtable             parameters = new() { { "@Id", id } };
    IEnumerable< Client > result     = await _net.StpAsync< Client >( "Client_GetDetails", parameters );

    return View( result.First() );
  }

  //GET Client/create
  public ActionResult Create() => View();

  //POST Client/create
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task< ActionResult > Create( [Bind( Include = "Name, Address, Suburb, State, ZipCode" )] Client model )
  {
    if ( !ModelState.IsValid ) return View( model );

    return RedirectToAction( "Index" );
  }

  //GET Client/edit
  public async Task< ActionResult > Edit( int id )
  {
    Hashtable             parameters = new() { { "@Id", id } };
    IEnumerable< Client > result     = await _net.StpAsync< Client >( "Client_GetDetails", parameters );

    return View( result.First() );
  }

  //POST Client/edit
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task< ActionResult > Edit(
    [Bind( Include = "Id,Name, Address, Suburb, State, ZipCode" )]
    Client model )
  {
    if ( !ModelState.IsValid ) return View( model );

    return RedirectToAction( "Index" );
  }

  //GET Client/delete
  public async Task< ActionResult > Delete( int id )
  {
    Hashtable             parameters = new() { { "@Id", id } };
    IEnumerable< Client > result     = await _net.StpAsync< Client >( "Client_GetDetails", parameters );

    return View( result.First() );
  }

  //POST Client/delete
  [HttpPost]
  [ActionName( "Delete" )]
  [ValidateAntiForgeryToken]
  public async Task< ActionResult > DeleteConfirmed( int Id ) => RedirectToAction( "Index" );
}