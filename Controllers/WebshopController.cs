using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using mtg_app.Models;
using mtg_app.Models.WebShop;
using mtg_app.Services;
using mtg_lib.Library.Services;

//using mtg_lib.Library.Services;

namespace mtg_app.Controllers;
public class WebshopController : Controller {
    
    //private ListingService ls = new ListingService();

    private CoinService coinService = new CoinService();
    
    private SessionService sessionService = new SessionService();
    
    private readonly int _BoosterPackPrice = 100;
    

        [Route("[action]")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("[action]")]
    public IActionResult ShowListingByListingId(){
        //parameter listingId toevoegen in functie
        //return View(ls.getListingbyListingId(listingId));
        return View();
    }

    [Route("[action]")]
    public IActionResult BuyBoosterPack(){
        
        int? amountOfPacks =  sessionService.GetAmountOfPacksCart(HttpContext.Session);
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int userCoinBalance = coinService.GetUserCoinBalance(userId);

        return View(new WebShopViewModel
        {
            AmountOfPacksInCart = amountOfPacks,
            CoinBalance = userCoinBalance,
            BoosterPackPrice = _BoosterPackPrice
        });
    }

    [HttpPost]

    public IActionResult AddToCart(WebShopViewModel webShopViewModel)
    {
        int amountOfPacksToAdd = webShopViewModel.AmountOfPacksToAdd;

        sessionService.StoreCartPacks(HttpContext.Session,amountOfPacksToAdd);

        return RedirectToAction("BuyBoosterPack");
    }
    
}