using d6Invoice.Models;
using d6Invoice.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace d6Invoice.Controllers
{
  public class ClientController : Controller
  {
    private readonly AdoNet _net;

    public ClientController()
    {
      _net = new AdoNet( ConfigurationManager.ConnectionStrings[ "DefaultConnectionString" ].ConnectionString );
    }


    //GET Client/index
    public async Task< ActionResult > Index( int page, int recordsPerPage )
    {
      Hashtable             parameters = new Hashtable { { "@page", page }, { "@recPerPage", recordsPerPage } };
      IEnumerable< Client > result     = await _net.StpAsync< Client >( "Client_Get", parameters );

      return View( result );
    }

    //GET Client/details
    public async Task< ActionResult > Details( int id )
    {
      Hashtable             parameters = new Hashtable { { "@Id", id } };
      IEnumerable< Client > result     = await _net.StpAsync< Client >( "Client_GetDetails", parameters );

      return View( result.First() );
    }

    //GET Client/create
    public ActionResult Create() { return View(); }

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
      Hashtable             parameters = new Hashtable { { "@Id", id } };
      IEnumerable< Client > result     = await _net.StpAsync< Client >( "Client_GetDetails", parameters );

      return View(result.First());
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
      Hashtable             parameters = new Hashtable { { "@Id", id } };
      IEnumerable< Client > result     = await _net.StpAsync< Client >( "Client_GetDetails", parameters );

      return View(result.First());
    }

    //POST Client/delete
    [HttpPost]
    [ActionName( "Delete" )]
    [ValidateAntiForgeryToken]
    public async Task< ActionResult > DeleteConfirmed( int Id ) { return RedirectToAction( "Index" ); }
  }
}