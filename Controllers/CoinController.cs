using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mtg_app.Models.Coin;
using mtg_lib.Library.Models;
using mtg_lib.Library.Services;

namespace mtg_app.Controllers
{
    
    [Route("[controller]")]
    public class CoinController : Controller
    {
        
        CoinService coinService = new CoinService();
        //private readonly SignInManager<IdentityUser> _signInManager = new SignInManager<IdentityUser>();

        [Authorize]
        [Route("")]
        [Route("[action]")]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            bool receivedFreeCoins = coinService.checkIfReceivedDefaultCoins(userId);
            bool receivedDailyCoins = coinService.checkDailyCoinsClaimed(userId);
            int userCoinBalance = coinService.GetUserCoinBalance(userId);
            
            
            return View(new CoinViewModel
            {
                UserCoinBalance = userCoinBalance,
                ReceivedDefaultCoins = receivedFreeCoins,
                ReceivedDailyCoins = receivedDailyCoins
            });
        }


        // /coin/freecoins/
        [Authorize]
        [Route("[action]")]
        public IActionResult ReceiveFreeCoins()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //Console.WriteLine("Current logged in user id: " + userId);

            coinService.receiveDefaultCoins(userId);
            
            return RedirectToAction("Index");
        }

        
        // /coin/receivedailycoins
        [Authorize]
        [Route("[action]")]
        public IActionResult ReceiveDailyCoins()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //Console.WriteLine("Current logged in user id: " + userId);
            
            
            return RedirectToAction("Index");
        }
        
    }
}