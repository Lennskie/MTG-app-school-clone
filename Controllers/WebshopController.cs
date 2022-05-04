using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mtg_app.Models;
//using mtg_lib.Library.Services;

namespace mtg_app.Controllers;
public class WebshopController : Controller {
    //private ListingService ls = new ListingService();

    [Route("[action]")]
    public IActionResult showListings(){
        //return View(ls.getListings());
        return View();
    }

    [Route("[action]")]
    public IActionResult showListingByListingId(){
        //parameter listingId toevoegen in functie
        //return View(ls.getListingbyListingId(listingId));
            return View();
    }

    [Route("[action]")]
    public IActionResult buyBoosterPack(){
        //parameter listingId toevoegen in functie
        //return View(ls.getListingbyListingId(listingId));
            return View();
    }
}