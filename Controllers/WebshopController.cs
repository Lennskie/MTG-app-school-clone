
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using mtg_app.Models.WebShop;
using mtg_app.Services;
using mtg_lib.Library.Services;


namespace mtg_app.Controllers;

[Route("[controller]")]
public class WebshopController : Controller {
    
    
    private readonly UserPackService _packService = new UserPackService();
    private readonly UserCoinService _userCoinService = new UserCoinService();
    private readonly SessionService _sessionService = new SessionService();
    
    private readonly int _BoosterPackPrice = 100;
    

    [Route("[action]")]
    public IActionResult Index()
    {
        return View();
    }

    
    [Route("[action]")]
    public IActionResult ShowListingByListingId(){
        return View();
    }

    
    [Route("[action]")]
    public IActionResult BuyBoosterPack() {
        
        int? amountOfPacks =  _sessionService.GetAmountOfPacksCart(HttpContext.Session);
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int userCoinBalance = _userCoinService.GetUserCoinBalance(userId);

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

        _sessionService.StoreCartPacks(HttpContext.Session,amountOfPacksToAdd);

        return RedirectToAction("BuyBoosterPack");
    }
    
    
    public IActionResult CleanCart()
    {
        _sessionService.ClearSession(HttpContext.Session);
        
        return RedirectToAction("BuyBoosterPack");
    }


    public IActionResult BuyPacksInCart(WebShopViewModel webShopViewModel)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int currentCoinBalance = _userCoinService.GetUserCoinBalance(userId);
        int currentAmountOfPacksInCart = _sessionService.GetAmountOfPacksCart(HttpContext.Session);
        int currentBoosterPackPrice = _BoosterPackPrice;
        int? totalCartPrice = currentBoosterPackPrice * currentAmountOfPacksInCart;
        

        if (totalCartPrice > currentCoinBalance)
        {
            ModelState.AddModelError("AmountOfPacksInCart", "Cart Contains to much packs!");
        }
        else
        {
            // Decrease coin balance
            _userCoinService.DecreaseUserCoinBalance(userId,totalCartPrice);
            
            // Buy the amount of packs if possible
            _packService.AddPackToUser(userId,currentAmountOfPacksInCart);
            
            // Clean Cart
            _sessionService.ClearSession(HttpContext.Session);
        }
        
        webShopViewModel.CoinBalance = _userCoinService.GetUserCoinBalance(userId);
        webShopViewModel.BoosterPackPrice = _BoosterPackPrice;
        webShopViewModel.AmountOfPacksInCart = _sessionService.GetAmountOfPacksCart(HttpContext.Session);
        
        return View("BuyBoosterPack", webShopViewModel);
    }
    
}