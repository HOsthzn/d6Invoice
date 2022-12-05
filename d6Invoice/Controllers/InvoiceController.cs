using d6Invoice.Models;
using d6Invoice.Utilities;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;

namespace d6Invoice.Controllers;

public class InvoiceController : Controller
{
  private readonly AdoNet _net;

  public InvoiceController()
  {
    _net = new AdoNet( ConfigurationManager.ConnectionStrings[ "DefaultConnection" ].ConnectionString );
  }

  //GET Invoice/index
  public ActionResult Index() => View();

  //GET Client/GetClients
  public async Task< ActionResult > GetInvoices( int? page, int? recPerPage, DateTime? date, int? clientId )
  {
    //Get all Client records from the DB
    Hashtable parameters = new() { { "@page", page }, { "@recPerPage", recPerPage } };

    InvoiceIndexViewModel result = new()
                                   {
                                     Page       = page
                                   , RecPerPage = recPerPage
                                   , Invoices   = await _net.StpAsync< InvoiceHeader >( "", parameters )
                                   , PageCount = ( await _net.StpAsync< PageCount >( ""
                                                  , new Hashtable { { "@recPerPage", recPerPage } } )
                                                 ).First()
                                                  .Count
                                   };

    return Json( result, JsonRequestBehavior.AllowGet );
  }

  //GET Invoice/details
  public async Task< ActionResult > Details( int id )
  {
    Hashtable                    parameters = new() { { "@Id", id } };
    IEnumerable< InvoiceHeader > result     = await _net.StpAsync< InvoiceHeader >( "Client_GetDetails", parameters );

    return View( result.First() );
  }

  //GET Invoice/create
  public ActionResult Create() => View();

  //POST Invoice/create
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task< ActionResult > Create( [Bind( Include = "" )] InvoiceHeader model )
  {
    if ( !ModelState.IsValid ) return View( model );

    try
    {
      Hashtable parameters = new() { };

      await _net.StpAsync< InvoiceHeader >( "", parameters );

      return RedirectToAction( "Index" );
    }
    catch ( Exception e )
    {
      ModelState.AddModelError( "Error", e.Message );
      return View( model );
    }
  }

  //GET Invoice/edit
  public async Task< ActionResult > Edit( int id )
  {
    Hashtable                    parameters = new() { };
    IEnumerable< InvoiceHeader > result     = await _net.StpAsync< InvoiceHeader >( "", parameters );

    return View( result.First() );
  }

  //POST Invoice/edit
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task< ActionResult > Edit( [Bind( Include = "" )] InvoiceHeader model )
  {
    if ( !ModelState.IsValid ) return View( model );
    try
    {
      Hashtable parameters = new() { };

      await _net.StpAsync< InvoiceHeader >( "", parameters );
      return RedirectToAction( "Index" );
    }
    catch ( Exception e )
    {
      ModelState.AddModelError( "Error", e.Message );
      return View( model );
    }
  }

  //GET Invoice/delete
  public async Task< ActionResult > Delete( int id )
  {
    Hashtable                    parameters = new() { };
    IEnumerable< InvoiceHeader > result     = await _net.StpAsync< InvoiceHeader >( "", parameters );

    return View( result.First() );
  }

  //POST Invoice/delete
  [HttpPost]
  [ActionName( "Delete" )]
  [ValidateAntiForgeryToken]
  public ActionResult DeleteConfirmed( int id )
  {
    _net.InLineStp( "", new Hashtable { { "@Id", id } } );
    return RedirectToAction( "Index" );
  }
}